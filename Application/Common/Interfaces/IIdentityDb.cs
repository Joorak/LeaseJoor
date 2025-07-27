namespace Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
public interface IIdentityDb {


    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
} 
