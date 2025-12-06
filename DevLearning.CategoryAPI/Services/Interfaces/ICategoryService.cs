using DevLearning.Models.DTOs.Category;

namespace DevLearning.CategoryAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task CreateCategoryAsync(CategoryRequestDTO categoryDto);
        Task<List<CategoryResponseDTO>> GetAllCategoriesAsync();
        Task<CategoryResponseDTO> GetCategoryByIdAsync(Guid id);
        Task UpdateCategoryAsync(Guid id, CategoryUpdateDTO categoryDto);
        Task DeleteCategoryAsync(Guid id);
        //Task<CategoryWithCoursesDTO> GetCategoryCoursesAsync(Guid categoryId);
    }
}
