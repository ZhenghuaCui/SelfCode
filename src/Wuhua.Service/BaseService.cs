using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Wuhua.IRepository;
using Wuhua.IService;

namespace Wuhua.Service
{
	public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
	{
		public IBaseRepository<TEntity> _iBaseRepository;
		public async Task<bool> CreatAsync(TEntity entity)
		{
			return await _iBaseRepository.CreatAsync(entity);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			return await _iBaseRepository.DeleteAsync(id);
		}
		public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> func)
		{
			return await _iBaseRepository.DeleteAsync(func);
		}

		public async Task<bool> EditAsync(TEntity entity)
		{
			return await _iBaseRepository.EditAsync(entity);
		}

		public async Task<TEntity> FindAsync(int id)
		{
			return await _iBaseRepository.FindAsync(id);
		}

		public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func)
		{
			return await _iBaseRepository.FindAsync(func);
		}

		public async Task<List<TEntity>> QueryAsync()
		{
			return await _iBaseRepository.QueryAsync();
		}

		public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func)
		{
			return await _iBaseRepository.QueryAsync(func);
		}

		public async Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total)
		{
			return await _iBaseRepository.QueryAsync(page, size, total);
		}

		public async Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int page, int size, RefAsync<int> total)
		{
			return await _iBaseRepository.QueryAsync(func, page, size, total);
		}
	}
}
