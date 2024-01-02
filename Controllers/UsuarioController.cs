using System;
using System.Collections;
using System.Collections.Generic;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;


namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario NovoUser)
        {
            NovoUser.Senha = Criptografo.TextoCriptografado(NovoUser.Senha);
            new UsuarioService().Inserir(NovoUser);

            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(string tipoFiltro, string filtro, int p=1)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            FiltrosUsuarios objFiltro = null;
            if(!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosUsuarios();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            int quantPag = 5;
            UsuarioService usuarioService = new UsuarioService();
            int totalRegistros = usuarioService.NumeroDeUsuarios();
            ICollection<Usuario> lista = usuarioService.ListarTodos (p, quantPag, objFiltro);
            ViewData["NumeroPag"] = (int) Math.Ceiling((double) totalRegistros / quantPag);
            return View(lista);
        }

        public IActionResult Edicao(int Id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            
            return View(new UsuarioService().ObterPorId(Id));
        }

        [HttpPost]
        public IActionResult Edicao(Usuario userEditado)
        {
            new UsuarioService().Atualizar(userEditado);
            return RedirectToAction("Listagem");
        }

        public IActionResult Excluir(int Id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            UsuarioService usuarioService = new UsuarioService();
            usuarioService.Excluir(Id);
            return RedirectToAction("Listagem");       
        }

        public IActionResult AdminError()
        {
            return View();
        }

    }
}