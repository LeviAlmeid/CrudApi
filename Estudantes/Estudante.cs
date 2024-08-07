namespace ApiCrud.Estudantes
{
    public class Estudante
    {
        public Guid Id { get; init; } //init pq não é alteravel | Guid gera um id aleatorio
        public string Nome { get; private set; } //apenas alteravel dentro da classe
        public bool Ativo { get; private set; }

        public Estudante(string nome)
        {
            Nome = nome;
            Id = Guid.NewGuid();
            Ativo = true;
        }

        public void AtualizarNome(string nome)
        {
            Nome = nome;
        }

        public void Desativar()
        {
            Ativo = false;
        }

    }
}
