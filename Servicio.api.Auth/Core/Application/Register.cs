using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicio.api.Auth.Core.Context;
using Servicio.api.Auth.Core.Dto;
using Servicio.api.Auth.Core.Entities;
using Servicio.api.Auth.Core.Entities.Jwt;

namespace Servicio.api.Auth.Core.Application;


public class Register
{
    public class UserRegisterCommand : IRequest<UserDto>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        // public string RoleName { get; set; }
        public string Password { get; set; }

    }

    public class UserRegisterValidation : AbstractValidator<UserRegisterCommand>
    {
        public UserRegisterValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            // RuleFor(x => x.RoleName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class UserRegisterHandler : IRequestHandler<UserRegisterCommand, UserDto>
    {
        private readonly SecurityContext _securityContext;
        private readonly UserManager<User> _usermanager;
        private readonly IMapper _mapper;

        private readonly IJwtGenerator _jwtgenerator;

        public UserRegisterHandler(SecurityContext context, UserManager<User> manager, IMapper mapper, IJwtGenerator jwtgenerator)
        {
            _securityContext = context;
            _usermanager = manager;
            _mapper = mapper;
            _jwtgenerator = jwtgenerator;
        }
        public async Task<UserDto> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var existe = _securityContext.Users.Where(p => p.Email == request.Email).Any();

            if (existe)
            {
                throw new Exception("El email del usuario ya existe en la base de datos");
            }

            existe = _securityContext.Users.Where(p => p.UserName == request.Username).Any();

            if (existe)
            {
                throw new Exception("El nombre de usuario ya existe en la base de datos");
            }

            var newUser = new User
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Username,
                RoleName = "Reader"
            };

            var resp = await _usermanager.CreateAsync(newUser, request.Password);
            await _usermanager.AddToRoleAsync(newUser, "Reader"); // Se crea como normal User. El admin se encarga de asignarle el role.


            if (resp.Succeeded)
            {
                var userDto = _mapper.Map<User, UserDto>(newUser);
                userDto.Token = _jwtgenerator.CreateToken(newUser);
                return userDto;
            }

            throw new Exception("No se pudo registrar el usuario");

        }
    }




}