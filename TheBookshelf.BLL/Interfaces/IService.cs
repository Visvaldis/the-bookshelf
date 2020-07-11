using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.Interfaces
{
	public interface IService<T> where T : class
	{
		ICollection<T> GetAll();
		T Get(int id);
		ICollection<T> GetByName(string name);
		int Add(T item);
		void Update(T item);
		void Delete(int id);
		bool Exist(int id);
		void Dispose();
	}
}
