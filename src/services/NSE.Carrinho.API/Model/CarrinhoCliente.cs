﻿using FluentValidation;
using FluentValidation.Results;

namespace NSE.Carrinho.API.Model
{
    //O carrinho cliente representa o carrinho do cliente 
    //então pra deixar claro, ele não representa o cliente
    public class CarrinhoCliente
    {
        internal const int MAX_QUANTIDADE_ITEM = 5;
        public Guid Id { get; set; }

        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }

        public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();

        public ValidationResult ValidationResult { get; set; }  

        public CarrinhoCliente(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
        }

        public CarrinhoCliente() { }

        internal void CalcularValorCarrinho()
        {
            ValorTotal = Itens.Sum(px => px.CalcularValor());
        }

        internal bool CarrinhoItemExistente(CarrinhoItem item)
        {
            return Itens.Any(p => p.ProdutoId == item.ProdutoId);
        }

        internal CarrinhoItem ObterPorProdutoId(Guid produtoId)
        {
            return Itens.FirstOrDefault(px => px.ProdutoId == produtoId);
        }

        internal void AdicionarItem(CarrinhoItem item)
        {            
            item.AssociarCarrinho(Id);

            if(CarrinhoItemExistente(item))
            {
                var itemExistente = ObterPorProdutoId(item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);

                item = itemExistente;
                Itens.Remove(itemExistente);
            }

            Itens.Add(item);
            CalcularValorCarrinho();
        }      
        
        internal void AtualizarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            var itemExistente = ObterPorProdutoId(item.ProdutoId);

            Itens.Remove(itemExistente);

            Itens.Add(item);

            CalcularValorCarrinho();
        }

        internal void AtualizarUnidades(CarrinhoItem item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        internal void RemoverItem(CarrinhoItem item)
        {
            var itemExistente = ObterPorProdutoId(item.ProdutoId);
            Itens.Remove(itemExistente);

            CalcularValorCarrinho();
        }

        internal bool EhValido()
        {
            var erros = Itens.SelectMany(px => new CarrinhoItem.ItemCarrinhoValidation().Validate(px).Errors).ToList();
            erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(erros);

            return ValidationResult.IsValid;
        }

        public class CarrinhoClienteValidation : AbstractValidator<CarrinhoCliente>
        {
            public CarrinhoClienteValidation()
            {
                RuleFor(px => px.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido");

                RuleFor(px => px.Itens.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens");

                RuleFor(px => px.ValorTotal)
                    .GreaterThan(0)
                    .WithMessage("O valor total do carrinho precisa ser maior que 0");
            }
        }

    }
}