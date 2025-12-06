using DevLearning.API.Models.DTOs.Author;
using DevLearning.Models;
using DevLearning.Models.Enums.Author;

namespace DevLearning.AuthorAPI.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<AuthorResponseDTO>> GetAllAuthorsAsync();
        Task<AuthorResponseDTO> GetAuthorByIdAsync(Guid id);
        Task<AuthorResponseDTO> GetAuthorByEmail(string email);
        Task CreateAuthorAsync(Author author);
        Task UpdatePatchAuthorAsync(UpdateAuthorParcialDTO dto, Guid id);
        Task UpdatePutAuthorAsync(UpdateAuthorFullDTO dto, Guid id);
        Task UpdateAuthorTypeAsync(Guid id, AuthorType type);
        Task<int> CountCoursesAsync(Guid id);
        //Task<(string AuthorName, List<string> Courses)> GetAuthorCoursesAsync(Guid authorId);
    }
}
