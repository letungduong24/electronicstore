using UserManagementAPI.DTOs;
using UserManagementAPI.Models;

namespace UserManagementAPI.Mapper
{
    public class UserMapper
    {
        public UserDto ToDTO(ApplicationUser model)
        {
            if (model == null)
                return null;

            var dto = new UserDto
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };

            return dto;
        }

        public UserDto ToDTO(ApplicationUser model, List<string> roles)
        {
            if (model == null)
                return null;

            var dto = new UserDto
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Roles = roles
            };

            return dto;
        }

        public ApplicationUser ToModel(UserDto dto)
        {
            if (dto == null)
                return null;

            var model = new ApplicationUser
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                UserName = dto.UserName,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            return model;
        }

        public ApplicationUser ToModel(RegisterDto dto)
        {
            if (dto == null)
                return null;

            return new ApplicationUser
            {
                Name = dto.Name,
                Email = dto.Email,
                UserName = dto.Email
            };
        }

        public void UpdateModel(UpdateUserDto dto, ApplicationUser model)
        {
            if (dto == null || model == null)
                return;

            model.Name = dto.Name;
            model.Address = dto.Address;
            model.PhoneNumber = dto.PhoneNumber;
        }
    }
} 