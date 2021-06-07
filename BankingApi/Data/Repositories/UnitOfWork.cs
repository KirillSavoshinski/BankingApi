using System.Threading.Tasks;
using AutoMapper;
using BankingApi.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankingApi.Data.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ITransactionRepository TransactionRepository => new TransactionRepository(_context);
        public IBankAccountRepository BankAccountRepository => new BankAccountRepository(_context, _mapper);
        
        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}