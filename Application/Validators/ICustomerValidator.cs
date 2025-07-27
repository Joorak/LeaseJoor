
namespace Application.Validators;
public interface ICustomerValidator
{
    Task<bool> IsValidEmail(string email);
    Task<bool> IsEmailUnique(string email);
}
