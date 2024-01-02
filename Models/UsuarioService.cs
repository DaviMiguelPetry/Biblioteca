using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models{

    public class UsuarioService{

        public void Inserir(Usuario user)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Add(user);
                bc.SaveChanges();
            }
            
        }

        public void Atualizar(Usuario user){

            using (BibliotecaContext bc = new BibliotecaContext())
            {
                Usuario usuario = bc.Usuarios.Find(user.Id);
                usuario.Login = user.Login;
                usuario.Nome = user.Nome;

                if(usuario.Senha != user.Senha)
                {
                    Criptografo.TextoCriptografado(user.Senha);
                }
                else
                {
                    usuario.Senha = user.Senha;
                }

                usuario.Tipo = user.Tipo;
                usuario.Id = user.Id;
                  
                bc.SaveChanges();
            }
        }

        public void Excluir(int Id)
        {
            using (BibliotecaContext bc = new BibliotecaContext()){
                bc.Usuarios.Remove(ObterPorId(Id));
                bc.SaveChanges();
            }
        }

        public ICollection<Usuario> ListarTodos (int pag = 1, int tam = 5, FiltrosUsuarios filtro = null)
        
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Usuario> query;
                int pular = (pag - 1) * tam;

                if(filtro != null){
                    switch(filtro.TipoFiltro)
                    {
                        case "Nome":
                            query = bc.Usuarios.Where(u => u.Nome.Contains(filtro.Filtro, StringComparison.CurrentCultureIgnoreCase));
                        break;

                        case "Login":
                            query = bc.Usuarios.Where(u => u.Login.Contains(filtro.Filtro, StringComparison.CurrentCultureIgnoreCase));
                        break;

                        default:
                            query = bc.Usuarios;
                        break;
                    }
                    }else{
                        query = bc.Usuarios;
                    }

                    return query.Skip(pular).Take(tam).ToList();
                }
            }
        

        public Usuario ObterPorId(int id){

            using (BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.Find(id);
            }
        }

        public int NumeroDeUsuarios(){
            using(var context = new BibliotecaContext()){
               return context.Usuarios.Count();
            }
        }
        }
}
