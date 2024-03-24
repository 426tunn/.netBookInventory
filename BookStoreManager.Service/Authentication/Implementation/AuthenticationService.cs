using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStoreManager.Contracts.Authentication;
using BookStoreManager.Data.Repo.intt;
using BookStoreManager.Domain.DTOs;
using BookStoreManager.Domain.Entities;
using BookStoreManager.Domain.Enum;
using BookStoreManager.Domain.Utils;
using BookStoreManager.Service.Authentication.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreManager.Service.Authentication.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IAuthorRepository _authorRepository;
        private readonly EncryptionService _encryptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IAuthorRepository authorRepository, IHttpContextAccessor httpContextAccessor, EncryptionService encryptionService)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _authorRepository = authorRepository;
            _encryptionService = encryptionService;
            _httpContextAccessor = httpContextAccessor;

        }
[Authorize(Roles = "Admin")]
 public async Task<string> DeleteAuthor(Guid Id)
 {
    //  var userId = ""; 
     try {
        //  if (_httpContextAccessor.HttpContext != null)
        //  {
        //      userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //      if (userId != Id.ToString())
        //      {
        //         throw new Exception("You are not authorized to delete this author.");
        //      }
        //  }     
      var authorExists = await _authorRepository.GetAuthorById(Id);
        if (authorExists == null)
        {
            throw new Exception("Author does not exist");
        }

        await _authorRepository.DeleteAuthor(Id);
        return "Author deleted successfully";

      } catch (Exception ex)
     {
          throw new Exception(ex.Message);
     }
 }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _authorRepository.GetAllAsync();
        }

        public Task<Author?> GetAuthor(Guid Id)
        {
            
           return _authorRepository.GetAuthorById(Id);
        }

        public async Task<ApiResponse> Login(userLogin author)
        {

            try {
                var FindAuthor = await _authorRepository.GetAuthor(author.Email);

               if (FindAuthor == null)
               {
                return new ApiResponse("invalid email", (int)HttpStatusCode.BadRequest, result: false);
               }

               bool isValidPassword = _encryptionService.VerifyPassword(author.Password, FindAuthor.Password);


               if (!isValidPassword){
                return new ApiResponse("invalid password", (int)HttpStatusCode.BadRequest, result: false);
               }
                //store user id from session
                var AuthIdString = _httpContextAccessor.HttpContext?.Session;
                if (AuthIdString == null){
                    throw new Exception("Author Session not found");
                }
                AuthIdString.SetString("AuthorId", FindAuthor.Id.ToString());
                var UserRole = await _authorRepository.GetUserRole(FindAuthor.Id);

                  //create jwt token
              var token = _jwtTokenGenerator.GeneratedToken(FindAuthor.Id, FindAuthor.Firstname, FindAuthor.Lastname, UserRole);

               return new ApiResponse("Login Successful", (int)HttpStatusCode.OK, token);

            } catch(Exception ex){
                throw new Exception (ex.Message);
            }
            
        }

        public async Task<string> Register(AuthorDTO author)
        {
            //Check if user doesnt exist
            var Filterauthor = await _authorRepository.GetAuthor(author.Email);
            
            if (Filterauthor is not null)
            {
                throw new Exception("Author with same email already exists");
            }

            var EncryptedPassword = _encryptionService.EncryptPassword(author.Password);

            author.Password = EncryptedPassword;

            await _authorRepository.CreateAuthor(author);
            return "registration successful";
           
        }

        public async Task<string>UpdateUserProfile(Guid id, UpdateProfileDTO profile)
        {
            try
            {
             var findUser = await _authorRepository.GetAuthorById(id);
            if (findUser == null)
            {
                throw new Exception("User not found");
            }
            bool needsUpdate = false;
            if (!string.IsNullOrEmpty(profile.Firstname) && profile.Firstname != findUser.Firstname)
            {
                needsUpdate = true;
            }
            if (!string.IsNullOrEmpty(profile.Lastname) && profile.Lastname != findUser.Lastname)
            {
                needsUpdate = true;
            }
            if (!string.IsNullOrEmpty(profile.Email) && profile.Email != findUser.Email)
            {
                needsUpdate = true;
            }

            if (!ValidationUtils.IsValidEmail(profile.Email))
            {
                throw new Exception("Invalid email");
            }
            if (!needsUpdate)
            {
                return "No changes made";
            }
            var result = await _authorRepository.UpdateProfile(id, profile);
            return "Profile updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
        }

        public async Task<string> UpdateUserRole(Guid id, string newRole)
        {

            try
            {
             var findUser = await _authorRepository.GetAuthorById(id);
            if (findUser == null)
            {
                throw new Exception("User not found");
            }
            if (newRole != "Admin" && newRole != "User"){
                throw new Exception("Invalid role");
            }
            findUser.Role = newRole;
            var newUserRole = _authorRepository.UpdateUserRole(id, newRole);
            return "User role updated successfully";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
      

        }
    }
}