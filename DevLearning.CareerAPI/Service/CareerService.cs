using Dapper;
using DevLearning.API.Models.DTOs.Carrer;
using DevLearning.API.Services;
using DevLearning.CareerAPI.Repository;
using DevLearning.CareerAPI.Repository.Interface;
using DevLearning.CareerAPI.Service.Interface;
using DevLearning.Models;
using DevLearning.Models.DTOs.Carrer;
using DevLearning.Models.Models.DTOs.CareerItem;
using System.Net.Http;

namespace DevLearning.CareerAPI.Service
{
    public class CareerService : ICareerService
    {
        public readonly ICareerRepository careerRepository;
        private readonly ILogger<CareerService> logger;

        private readonly HttpClient _httpClientCourse;
        public CareerService(ILogger<CareerService> logger, ICareerRepository careerRepository, HttpClient httpClientCourse)
        {
            this.careerRepository = careerRepository;
            this.logger = logger;
            _httpClientCourse = httpClientCourse;
        }

        public async Task CreateCareerAsync(CareerRequestDTO careerDTO)
        {
            try

            {
                bool retorno = await careerRepository.GetCareerByTitleAsync(careerDTO.Title);
                if (retorno is true)
                {
                    throw new Exception("Já existe uma carreira com esse título.");
                }

                var career = new Career(
                   careerDTO.Title,
                   careerDTO.Summary,
                   careerDTO.Title.ToLower().Replace(" ", "-"),
                   careerDTO.DurationInMinutes,
                   careerDTO.Tags
                );

                foreach (var itemDTO in careerDTO.careerItems)
                {
                    //Mtodo auxiliar para ver se o curso existe na outra API

                    var apiCourse = await GetCourseFromExternalApi(itemDTO.CourseId);

                    if (apiCourse == null)
                        throw new Exception($"Curso {itemDTO.CourseId} não encontrado.");

                    var careerItems = new CareerItem(
                        career.Id,
                        itemDTO.CourseId,
                        itemDTO.Title,
                        itemDTO.Description,
                        itemDTO.Order
                    );

                    career.AddItem(careerItems);
                }

                await careerRepository.CreateCareerAsync(career);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao criar carreira e item carreira: {ex.Message}");
                throw;
            }

                    /*var careerItems = careerDTO.careerItems.Select(itemDTO => new CareerItem(
                    career.Id,
                    itemDTO.CourseId,
                    itemDTO.Title,
                    itemDTO.Description,
                    itemDTO.Order
                )).ToList();
                foreach (var item in careerItems)
                {
                    career.AddItem(item);
                }*/
        }
        public async Task<List<CareerWhitCareerItemResponseDTO>> GetAllCareerAsync()
        {
            try
            {
                var careers = await careerRepository.GetAllCareerWithCareerItem();
                if (careers.Count == 0)
                {
                    throw new Exception("Ainda não há nenhuma carreira cadastrada");
                }

                // Preenche o nome do curso via API
                await FillCourseAttributes(careers);

                return careers;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao listar todas as carreiras: {ex.Message}");
                throw;
            }
        }

        public async Task<CareerWhitCareerItemResponseDTO?> GetCareerByIdAsync(Guid careerId)
        {
            try
            {
                var career = await careerRepository.GetOneCareerWithCareerItem(careerId);
                if (career == null)
                {
                    throw new Exception("Carreira não encontrada");
                }

                await FillCourseAttributes(new List<CareerWhitCareerItemResponseDTO> { career });

                return career;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao buscar carreira por ID: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteCareerAsync(Guid careerId)
        {
            try
            {
                var career = await careerRepository.GetOneCareerWithCareerItem(careerId);
                if (career == null)
                {
                    throw new Exception("Carreira não encontrada");
                }

                var result = await careerRepository.DeleteCareerAsync(careerId);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao deletar carreira: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateCareerAsync(Guid id, CareerUpdateDTO updateDTO)
        {
            try
            {

                var existingCareer = await careerRepository.GetOneCareerWithCareerItem(id);
                if (existingCareer == null)
                {
                    throw new Exception("Carreira não encontrada.");
                }


                var updates = new List<string>();
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);

                if (!string.IsNullOrEmpty(updateDTO.Title))
                {
                    bool retorno = await careerRepository.GetCareerByTitleAsync(updateDTO.Title);
                    if (retorno is true)
                    {
                        throw new Exception("Já existe uma carreira com esse título.");
                    }

                    updates.Add("Title = @Title");
                    parameters.Add("Title", updateDTO.Title);
                    updates.Add("Url = @Url");
                    parameters.Add("Url", updateDTO.Title.ToLower().Replace(" ", "-"));
                }

                if (!string.IsNullOrEmpty(updateDTO.Summary))
                {
                    updates.Add("Summary = @Summary");
                    parameters.Add("Summary", updateDTO.Summary);
                }

                if (updateDTO.DurationInMinutes.HasValue)
                {
                    updates.Add("DurationInMinutes = @DurationInMinutes");
                    parameters.Add("DurationInMinutes", updateDTO.DurationInMinutes.Value);
                }

                if (updateDTO.Active.HasValue)
                {
                    updates.Add("Active = @Active");
                    parameters.Add("Active", updateDTO.Active.Value);
                }

                if (updateDTO.Featured.HasValue)
                {
                    updates.Add("Featured = @Featured");
                    parameters.Add("Featured", updateDTO.Featured.Value);
                }

                if (!string.IsNullOrEmpty(updateDTO.Tags))
                {
                    updates.Add("Tags = @Tags");
                    parameters.Add("Tags", updateDTO.Tags);
                }



                var result = await careerRepository.UpdateCareerAsync(id, updates, parameters);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao atualizar carreira: {ex.Message}");
                throw;
            }
        }


        //percorre a lista de carreiras e itens para preencher os atributos do curso via API externa
        public async Task FillCourseAttributes(List<CareerWhitCareerItemResponseDTO> careers)
        {
            foreach (var career in careers)
            {
                foreach (var item in career.Items)
                {
                    if (string.IsNullOrEmpty(item.CourseId)) continue;

                    try
                    {
                        var courseDto = await GetCourseFromExternalApi(item.CourseId);

                        if (courseDto != null)
                        {
                            item.CourseTitle = courseDto.Title; // Preenche o que o SQL não trouxe
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning($"Falha ao buscar dados do curso {item.CourseId}: {ex.Message}");
                    }
                }
            }
        }

        //metodo auxiliar para buscar o curso na API externa
        public async Task<APICourseDTO> GetCourseFromExternalApi(string courseId)
        {
            try
            {
                return await _httpClientCourse.GetFromJsonAsync<APICourseDTO>(courseId);
            }
            catch 
            {
                return null;
            } 
        }
    }
}
