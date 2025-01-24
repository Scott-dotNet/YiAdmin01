using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiAdmin01.BLL.Services.OrganizationManage;
using YiAdmin01.Common.Configs;
using YiAdmin01.Entity;
using YiAdmin01.Model.Param;
using YiAdmin01.Model;
using YiAdmin01.Common.Extension;

namespace YiAdmin01.BLL.Business
{
    /// <summary>
    /// NewsBLL 类是一个业务逻辑层（BLL）类，用于处理与新闻相关的业务逻辑。
    /// 它依赖于 NewsService 类来与数据层进行交互，
    /// 并使用 AreaBLL 类来处理与区域相关的逻辑
    /// </summary>
    public class NewsBLL
    {
        private AreaBLL areaBLL = new AreaBLL();
        private NewsService newsService = new NewsService();

        #region 获取数据
        /// <summary>
        /// 获取新闻列表。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TData<List<NewsEntity>>> GetList(NewsListParam param)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            areaBLL.SetAreaParam(param);
            obj.Data = await newsService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<NewsEntity>>> GetPageList(NewsListParam param, Pagination pagination)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            areaBLL.SetAreaParam(param);
            obj.Data = await newsService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<NewsEntity>>> GetPageContentList(NewsListParam param, Pagination pagination)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            obj.Data = await newsService.GetPageContentList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<NewsEntity>> GetEntity(long id)
        {
            TData<NewsEntity> obj = new TData<NewsEntity>();
            obj.Data = await newsService.GetEntity(id);
            areaBLL.SetAreaId(obj.Data);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Data = await newsService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 根据新闻标题搜索新闻列表。
        /// </summary>
        /// <param name="title">新闻标题</param>
        /// <returns>新闻列表</returns>
        public async Task<TData<List<NewsEntity>>> SearchNewsByTitle(string title)
        {
            TData<List<NewsEntity>> obj = new TData<List<NewsEntity>>();
            var param = new NewsListParam { NewsTitle = title };
            obj.Data = await newsService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(NewsEntity entity)
        {
            TData<string> obj = new TData<string>();
            areaBLL.SetAreaEntity(entity);
            await newsService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await newsService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
