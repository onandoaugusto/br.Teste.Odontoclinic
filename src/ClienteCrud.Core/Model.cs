namespace ClienteCrud.Core.Model
{
    public abstract class Table
    {
        public virtual int Id { get; set; }
        public virtual DateTime DtCriacao { get; set; }
        public virtual bool Ativo { get; set; }   
    }

    public class Cliente : Table
    {
        public virtual string Nome { get; set; }
        public virtual string Sexo { get; set; }
        public virtual string Endereco { get; set; }
        public virtual List<Telefone> Telefones { get; set; }
    }

    public class Telefone : Table
    {
        public virtual string Numero { get; set; }

        public virtual Cliente cliente { get; set; }
    }
}

