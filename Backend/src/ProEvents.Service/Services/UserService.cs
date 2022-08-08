using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEvents.Domain.Identity;
using ProEvents.Infra.Interface;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.Service.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMapper mapper,
        IUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<bool> UserExists(string username)
    {
        try
        {
            return await _userManager.Users
                .AnyAsync(user => user.UserName == username.ToLower());
        }
        catch (Exception ex)
        {
            throw new Exception($"Error user not exist. Error: {ex.Message}");
        }
    }

    public async Task<UserUpdateDTO> GetUserByUsernameAsync(string username)
    {
        try
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user != null)
                return _mapper.Map<UserUpdateDTO>(user);

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error to find a User by Username. Error: {ex.Message}");
        }
    }
    public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpdateDto, string password)
    {
        try
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.Username.ToLower());

            // false: para não bloquear a conta caso não for validado o usuário com a senha
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error to check the password. Error: {ex.Message}");
        }
    }

    public async Task<UserDTO> CreateAccountAsync(UserDTO userDto)
    {
        try
        {
            var user = _mapper.Map<User>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                var userReturn = _mapper.Map<UserDTO>(user);
                return userReturn;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error to create a User. Error: {ex.Message}");
        }
    }

    public async Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDto)
    {
        try
        {
            var user = await _userRepository.GetUserByUsernameAsync(userUpdateDto.Username);
            if (user == null) return null;

            userUpdateDto.Id = user.Id;
            _mapper.Map(userUpdateDto, user);

            if (userUpdateDto.Password != null)
            {
                // Alterando o Password e dando Reset no Token (atualizando o token)
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                // Passa o token p/ caso alterar a senha não deslogar o usuário
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
            }

            await _userRepository.Update(user);

            var userReturn = await _userRepository.GetUserByUsernameAsync(user.UserName);
            return _mapper.Map<UserUpdateDTO>(userReturn);

        }
        catch (Exception ex)
        {
            throw new Exception($"Error to update a User. Error: {ex.Message}");
        }
    }

    public void DeleteImage(string imageName, string path)
    {
        if (imageName != null)
        {
            var imgPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @$"Resources/{path}",
                imageName
            );

            if (System.IO.File.Exists(imgPath))
                System.IO.File.Delete(imgPath);
        }
    }

    public async Task<string> SaveImage(IFormFile imgFile, string path)
    {
        // Definindo o nome do Arquivo
        string imgName = new String(
            Path.GetFileNameWithoutExtension(imgFile.FileName)
            .Take(10) // pega os 10 primeiros caracteres
            .ToArray()
            ).Replace(" ", "-"); // Caso tiver espaço, substitui por "-"

        imgName = $"{imgName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imgFile.FileName)}";

        var imgPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            @$"Resources/{path}",
            imgName
        );

        using (var fileStream = new FileStream(imgPath, FileMode.Create))
        {
            await imgFile.CopyToAsync(fileStream);
        }

        return imgName;
    }
}

