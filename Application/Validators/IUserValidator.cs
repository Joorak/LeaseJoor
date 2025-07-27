
namespace Application.Validators; 
public interface IUserValidator<User> {
    Task<bool> IsUsernameUnique(string email);
} 
