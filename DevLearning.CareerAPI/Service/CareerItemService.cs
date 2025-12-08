using Dapper;
using DevLearning.CareerAPI.Repository;
using DevLearning.CareerAPI.Repository.Interface;
using DevLearning.CareerAPI.Service.Interface;
using DevLearning.Models;
using DevLearning.Models.DTOs.CareerItem;
using DevLearning.Models.Models.DTOs.CareerItem;


namespace DevLearning.CareerAPI.Service
{
    public class CareerItemService : ICareerItemService
    {
        public readonly ICareerItemRepository careerItemRepository;
        public readonly ICareerRepository career;
        private readonly ILogger<CareerItemService> logger;

        private readonly HttpClient _httpClientCourse;
        public CareerItemService(ILogger<CareerItemService> logger, ICareerItemRepository careerItemRepository, 
            ICareerRepository career, HttpClient httpClientCourse)
        {
            this.careerItemRepository = careerItemRepository;
            this.logger = logger;
            this.career = career;
            _httpClientCourse = httpClientCourse;
        }

        public async Task<bool> CreateItemCareerAsync(CareerItemRequestCreateDTO careerItemDTO)
        {
            try
            {
                var retorno = await careerItemRepository.GetCareerItemByIdAsync(careerItemDTO.CareerId, careerItemDTO.CourseId);
                if (retorno == true)
                {
                    throw new Exception("Este curso já está cadastrado nesta carreira");
                }

                retorno = await careerItemRepository.GetCareerItemByTitleAsync(careerItemDTO.Title);
                if (retorno == true)
                {
                    throw new Exception("Já existe uma carreira com esse título");
                }

                retorno = await careerItemRepository.ExistingCarrerItemwhitOrder(careerItemDTO.Order);
                if (retorno == true)
                {
                    throw new Exception("Já existe uma carreira com essa ordem");
                }

                var apiCourse = await GetCourseFromExternalApi(careerItemDTO.CourseId);

                if (apiCourse == null)
                {
                    throw new Exception($"Curso com ID {careerItemDTO.CourseId} não encontrado na API de Cursos.");
                }

                var careerItems = new CareerItem(
                    careerItemDTO.CareerId,
                    careerItemDTO.CourseId,
                    careerItemDTO.Title,
                    careerItemDTO.Description,
                    careerItemDTO.Order
                );

                await careerItemRepository.CreateCareerItemAsync(careerItems);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao item carreira: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteItemCareerAsync(Guid careerId, string courseId)
        {
            try
            {
                var retorno = await careerItemRepository.GetCareerItemByIdAsync(careerId, courseId);
                if (retorno == false)
                {
                    throw new Exception("Item de carreira não encontrado");
                }
                var result = await careerItemRepository.DeleteCareerItemAsync(careerId, courseId);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao deletar item de carreira: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateCareerItemAsync(Guid careerId, string courseId, CareerItemUpdateDTO updateDTO)
        {
            try
            {
                bool retorno = await careerItemRepository.GetCareerItemByIdAsync(careerId, courseId);
                if (retorno == false)
                {
                    throw new Exception("Item de carreira não encontrado");
                }

                var updates = new List<string>();
                var parameters = new DynamicParameters();
                parameters.Add("CareerId", careerId);
                parameters.Add("CourseId", courseId);


                if (!string.IsNullOrEmpty(updateDTO.CourseId))
                {
                    //Valido se esse NOVO ID existe na API externa antes de salvar
                    var apiCourse = await GetCourseFromExternalApi(updateDTO.CourseId);

                    if (apiCourse == null)
                    {
                        throw new Exception($"O novo curso (ID: {updateDTO.CourseId}) não existe na base de dados.");
                    }

                    updates.Add("CourseId = @NewCourseId");
                    parameters.Add("NewCourseId", updateDTO.CourseId);
                }

                    if (!string.IsNullOrEmpty(updateDTO.Title))
                    {
                        retorno = await careerItemRepository.GetCareerItemByTitleAsync(updateDTO.Title);
                        if (retorno == true)
                        {
                            throw new Exception("Já existe uma carreira com esse título");
                        }
                        updates.Add("Title = @Title");
                        parameters.Add("Title", updateDTO.Title);
                    }
                    if (!string.IsNullOrEmpty(updateDTO.Description))
                    {
                        updates.Add("Description = @Description");
                        parameters.Add("Description", updateDTO.Description);
                    }
                    if (updateDTO.Order.HasValue)
                    {
                        updates.Add("[Order] = @Order");
                        parameters.Add("Order", updateDTO.Order);
                    }

                    var result = await careerItemRepository.UpdateCareerItemAsync(careerId, courseId, updates, parameters);
                    return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Erro interno ao atualizar carreira: {ex.Message}");
                throw;
            }
        }

        /*Aqui onde comunico com Course*/
        public async Task<APICourseDTO> GetCourseFromExternalApi(string courseId)
        {
            try
            {
                return await _httpClientCourse.GetFromJsonAsync<APICourseDTO>(courseId);  //envia um get para a api externa, pega a resposta em Json e desserializa para APICourseDTO
            }
            catch { return null; }
        }
    }
}
