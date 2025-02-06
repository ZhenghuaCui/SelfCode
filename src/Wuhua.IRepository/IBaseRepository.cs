using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Wuhua.IRepository
{
	public interface IBaseRepository<TEntity> where TEntity : class, new()
	{
		Task<bool> CreatAsync(TEntity entity);
		Task<bool> DeleteAsync(int id);
		Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression);
		Task<bool> EditAsync(TEntity entity);
		Task<TEntity> FindAsync(int id);
		Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> func);
		/// <summary>
		/// ��ѯȫ������
		/// </summary>
		/// <returns></returns>
		Task<List<TEntity>> QueryAsync();

		/// <summary>
		/// ��ѯȫ������
		/// </summary>
		/// <returns></returns>
		Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func);

		/// <summary>
		/// ��ѯȫ������
		/// </summary>
		/// <returns></returns>
		Task<List<TEntity>> QueryAsync(int page, int size, RefAsync<int> total);

		/// <summary>
		/// ��ѯȫ������
		/// </summary>
		/// <returns></returns>
		Task<List<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> func, int page, int size, RefAsync<int> total);

	}
}
