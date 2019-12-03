using FluentValidator.Validation;
using VerityNet.Shared.ValueObjects;

namespace VerityNet.Domain.MovimentoManualContext.ValueObjects
{
    public class UsuarioVO : ValueObject
    {
        public UsuarioVO() { }

        public UsuarioVO(string cod_Usuario)
        {
            Cod_Usuario = cod_Usuario;

            AddNotifications(new ValidationContract()
                .Requires()
                .HasMaxLen(cod_Usuario, 20, "Usuario", "O campo usuario deve conter no máximo 20 caracteres")
                .HasMinLen(cod_Usuario, 3, "Usuario", "O campo usuario deve conter no mínimo 3 caracteres")
            );
        }

        public string Cod_Usuario { get; private set; }

        public override string ToString() =>
           $"[ { GetType().Name } -> Usuário: { Cod_Usuario } ]";
    }
}
