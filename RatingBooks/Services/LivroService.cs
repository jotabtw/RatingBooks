﻿using AutoMapper;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RatingBooks.Data;
using RatingBooks.Data.Dtos.LivroDtos;
using RatingBooks.Models;

namespace RatingBooks.Services
{
    public class LivroService
    {
        private readonly IMapper _mapper;
        private readonly UsuarioDbContext _context;
        public LivroService(IMapper mapper, UsuarioDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<string> CriarLivro(CreateLivroDto livroDto, string userId)
        {
            Livro livro = _mapper.Map<Livro>(livroDto);

            livro.UsuarioId = userId;

            _context.Livros.Add(livro);

            await _context.SaveChangesAsync();

            return "Livro Adicionado com sucesso!";
        }

        public async Task<string> DeletarLivro(DeleteLivroDto livroDto, string userId)
        {
            Livro livro = await _context.Livros.SingleOrDefaultAsync(lvr => lvr.Id == livroDto.Id);

            if (livro is null || !(livro.UsuarioId == userId))
                return "Solicitação inválida";

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();
            return "Livro deletado!";
        }

        public async Task<List<GetLivroDto>> GetAllLivros(string userId) 
        {
            List<Livro> Livros = await _context.Livros.Where( lvr => lvr.UsuarioId == userId).ToListAsync();
            if (Livros.Count <= 0)
                return null;

            List<GetLivroDto> livrosDto = _mapper.Map<List<GetLivroDto>>(Livros);

            return livrosDto;
        }

        public async Task<List<GetLivroDto>> GetByNameLivro (string tituloLivro,string userId)
        {
            List<Livro> livro = await _context.Livros.Where(lvr => lvr.UsuarioId == userId).Where(lvr => lvr.Titulo == tituloLivro).ToListAsync();
            if (livro.Count <= 0)
                return null;

            List<GetLivroDto> livroDto = _mapper.Map<List<GetLivroDto>>(livro);

            return livroDto;
        }

        public async Task<GetLivroDto> GetById(int id, string userId)
        {
            Livro livro = await _context.Livros.Where(lvr => lvr.Id == id).Where(lvr => lvr.UsuarioId == userId).FirstOrDefaultAsync();
            if (livro is null)
                return null;

            GetLivroDto livroDto = _mapper.Map<GetLivroDto>(livro);

            return livroDto;
        }

        public async Task<Livro> AtualizarLivro(int id, string userId, UpdateLivroDto livroUpdate)
        {
            Livro livro = await _context.Livros.Where(lvr => lvr.Id == id).Where(lvr => lvr.UsuarioId == userId).FirstOrDefaultAsync();
            if (livro is null)
                return null;

            var livroDto = _mapper.Map(livroUpdate, livro);
            await _context.SaveChangesAsync();

            //AgendamentoTeste(livroDto);

            return livroDto;
        }

        //public async Task AgendamentoTeste(Livro livro) 
        //{
        //    if (!livro.Status == "Finalizado")
        //        return;
            
        //}
    }
}
