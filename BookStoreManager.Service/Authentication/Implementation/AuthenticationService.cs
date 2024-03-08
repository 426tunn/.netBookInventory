using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BookStoreManager.Contracts.Authentication;
using BookStoreManager.Data.Repo.intt;
using BookStoreManager.Domain.DTOs;
using BookStoreManager.Domain.Entities;
using BookStoreManager.Service.Authentication.Interface;
using Microsoft.AspNetCore.Http;

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

        public async Task<string> DeleteAuthor(Guid Id)
        {
           try {
            var authorExists = await _authorRepository.GetAuthorById(Id);
            if (authorExists == null)
            {
                throw new Exception("Author does not exist");
            }

            await _authorRepository.DeleteAuthor(Id);
             await _authorRepository.SaveChangesAsync();
            return "Author deleted successfully";

           } catch(Exception ex) {
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

                  //create jwt token
              var token = _jwtTokenGenerator.GeneratedToken(FindAuthor.Id, FindAuthor.Firstname, FindAuthor.Lastname);

               return new ApiResponse("Login Successful", (int)HttpStatusCode.OK, token);

            } catch(Exception ex){
                throw new Exception (ex.Message);
            }
            
        }

        public async Task<string> Register(AuthorDTO author/*Guid Id, string Firstname, string Lastname, string Email, string Password*/)
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

            //create user(generate id and token)
            // var Token = _jwtTokenGenerator.GeneratedToken(author.Id, Firstname, Lastname);
            return "registration successful";
           
        }
    }
}