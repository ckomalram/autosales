using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicio.api.Auth.Core.Context;
using Servicio.api.Auth.Core.Entities;

namespace Servicio.api.Auth.Core.Application;


public class Role
{
    public class UpdateUserRoleCommand : IRequest
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }

    }

    public class UpdateUserRoleValidation : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleValidation()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.RoleName).NotEmpty();
        }
    }

    public class UpdateUserRoleHandler : IRequestHandler<UpdateUserRoleCommand>
    {
        private readonly SecurityContext _securityContext;
        private readonly UserManager<User> _usermanager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly IMapper _mapper;


        public UpdateUserRoleHandler(SecurityContext context, UserManager<User> manager, IMapper mapper, RoleManager<IdentityRole> rolemanager)
        {
            _securityContext = context;
            _usermanager = manager;
            _mapper = mapper;
            _rolemanager = rolemanager;
        }
        public async Task<Unit> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _usermanager.FindByIdAsync(request.UserId.ToString());


            if (user == null)
            {
                throw new Exception($"No se encontró el usuario con ID '{request.UserId}'.");
            }


            var newRoleName = request.RoleName ?? "Reader";
            var role = await _rolemanager.FindByNameAsync(newRoleName);


            if (role == null)
            {
                throw new Exception($"No se encontró el rol '{newRoleName}'.");
            }

            var currentRoles = await _usermanager.GetRolesAsync(user);
            await _usermanager.RemoveFromRolesAsync(user, currentRoles);
            user.RoleName = role.Name;
            await _usermanager.UpdateAsync(user);
            var resp = await _usermanager.AddToRoleAsync(user, role.Name);


            if (resp.Succeeded)
            {
                return Unit.Value;
            }

            throw new Exception("No se pudo actualizar el rol del usuario");

        }
    }




}