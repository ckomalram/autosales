using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicio.api.Auth.Core.Context;
using Servicio.api.Auth.Core.Dto;
using Servicio.api.Auth.Core.Entities;
using Servicio.api.Auth.Core.Entities.Jwt;

namespace Servicio.api.Auth.Core.Application;


public class Login
{
    public class UserLoginCommand : IRequest<UserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class UserLoginValidation : AbstractValidator<UserLoginCommand>
    {
        public UserLoginValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();

        }
    }

    public class UserLoginHandler : IRequestHandler<UserLoginCommand, UserDto>
    {
        private readonly SecurityContext _securityContext;
        private readonly UserManager<User> _usermanager;
        private readonly IMapper _mapper;

        private readonly IJwtGenerator _jwtgenerator;
        private readonly SignInManager<User> _signinmanager;


        public UserLoginHandler(SecurityContext context, UserManager<User> manager, IMapper mapper, IJwtGenerator jwtgenerator, SignInManager<User> signinmanager)
        {
            _securityContext = context;
            _usermanager = manager;
            _mapper = mapper;
            _jwtgenerator = jwtgenerator;
            _signinmanager = signinmanager;
        }

        public async Task<UserDto> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _usermanager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new Exception("El usuario no exise...");
            }

            // Validar login
            var rta = await _signinmanager.CheckPasswordSignInAsync(user, request.Password, false);

            if (rta.Succeeded)
            {
                var userRta = _mapper.Map<User, UserDto>(user);

                userRta.Token = _jwtgenerator.CreateToken(user);

                return userRta;
            }


            throw new Exception("Login fallido...");


        }

    }




}