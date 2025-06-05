using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;

namespace ClienteCrud.Infra
{
    public class ClienteMap : ClassMap<Core.Model.Cliente>
    {
        public ClienteMap()
        {
            Table("Clientes");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DtCriacao).Not.Nullable();
            Map(x => x.Ativo).Not.Nullable();

            Map(x => x.Nome).Not.Nullable();
            Map(x => x.Sexo);
            Map(x => x.Endereco);
            HasMany(x => x.Telefones)
            .KeyColumn("ClienteId")
            .Cascade.AllDeleteOrphan() //TODO: Avaliar necessidade real de exclusão em cascata
            .Inverse();
        }
    }

    public class TelefoneMap : ClassMap<Core.Model.Telefone>
    {
        public TelefoneMap()
        {
            Table("Telefones");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DtCriacao).Not.Nullable();
            Map(x => x.Ativo).Not.Nullable();

            Map(x => x.Numero);

            References(x => x.cliente).Column("ClienteId");
        }
    }
}
