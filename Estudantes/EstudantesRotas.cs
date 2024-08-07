using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes;


    public static class EstudantesRotas 
    {
        public static void AddRotasEstudantes(this WebApplication app)
        {
            var rotasEstudantes = app.MapGroup("estudantes");


            //criar
            rotasEstudantes.MapPost("", 
                async(AddEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
            {
                var existeEstudante = await context.Estudantes
                .AnyAsync(estudante => estudante.Nome == request.Nome, ct);

                if (existeEstudante)
                    return Results.Conflict("Este estudante já existe!");


                var novoEstudante = new Estudante(request.Nome);

                await context.Estudantes.AddAsync(novoEstudante, ct);
                await context.SaveChangesAsync(ct); //precisa sempre para salvar

                var estudanteRetorno = new EstudanteDto(novoEstudante.Id, novoEstudante.Nome);

                return Results.Ok(estudanteRetorno);
            });

            //listar estudantes cadastrados
            rotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken ct) =>
            {
                var estudante = await context
                .Estudantes
                .Where(estudante => estudante.Ativo)
                .Select(estudante => new EstudanteDto(estudante.Id, estudante.Nome))
                .ToListAsync(ct);
                return estudante;
            });


            //Atualizar Nome
            rotasEstudantes.MapPut("{id}", async (Guid Id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
            {
                var estudante = await context.Estudantes
                    .SingleOrDefaultAsync(estudante => estudante.Id == Id, ct);

                if (estudante == null)
                    return Results.NotFound();

                estudante.AtualizarNome(request.Nome);

                await context.SaveChangesAsync(ct);

                return Results.Ok(new EstudanteDto(estudante.Id, estudante.Nome));
            });


            //Delete
            rotasEstudantes.MapDelete("{id}", async (Guid id, AppDbContext context, CancellationToken ct) =>
            {
                var estudante = await context.Estudantes
                    .SingleOrDefaultAsync(estudante => estudante.Id == id, ct);

                if (estudante == null)
                    return Results.NotFound();

                estudante.Desativar();

                return Results.Ok();
            });

        }
    }

