using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class EmprestimoService 
    {
        public void Inserir(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Emprestimos.Add(e);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Emprestimo emprestimo = bc.Emprestimos.Find(e.Id);
                emprestimo.NomeUsuario = e.NomeUsuario;
                emprestimo.Telefone = e.Telefone;
                emprestimo.LivroId = e.LivroId;
                emprestimo.DataEmprestimo = e.DataEmprestimo;
                emprestimo.DataDevolucao = e.DataDevolucao;

                bc.SaveChanges();
            }
        }

        public ICollection<Emprestimo> ListarTodos(FiltrosEmprestimos filtro = null)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Emprestimo> query;
                
                if(filtro != null)
                {
                    
                    switch(filtro.TipoFiltro)
                    {
                        case "Usuario":
                            query = bc.Emprestimos.Include (e => e.Livro).Where (e => e.NomeUsuario.Contains(filtro.Filtro));
                        break;

                        case "Livro":
                           query = bc.Emprestimos.Include (e => e.Livro).Where (e => e.Livro.Titulo.Contains(filtro.Filtro));
                        break;

                        default:
                            query = bc.Emprestimos.Include (e => e.Livro);
                        break;
                    }
                }else{
                    query = bc.Emprestimos.Include (e => e.Livro);
                }

                return query.OrderByDescending(e => e.DataDevolucao).ToList();
            }
        }

        public Emprestimo ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Emprestimos.Find(id);
            }
        }
    }
}

     //       public ICollection<Livro> ListarTodos(FiltrosLivros filtro = null)
     //   {
       //     using(BibliotecaContext bc = new BibliotecaContext())
       //     {
       //         IQueryable<Livro> query;
                
        //        if(filtro != null)
         //       {
                    //definindo dinamicamente a filtragem
          //          switch(filtro.TipoFiltro)
            //        {
             //           case "Autor":
             ///               query = bc.Livros.Where(l => l.Autor.Contains(filtro.Filtro));
              //          break;

                //        case "Titulo":
                 //           query = bc.Livros.Where(l => l.Titulo.Contains(filtro.Filtro));
                 //       break;

                  //      default:
                   //         query = bc.Livros;
                  //      break;
                  //  }