using Pharos.Logic.BLL.DataSynchronism;
using Pharos.Logic.DAL;
using Pharos.Logic.DAL.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Pharos.Logic.BLL.LocalServices.DataSync
{
    public class BaseLocalDataSyncService
    {

        /// <summary>
        /// 从本地同步到服务器上，获取数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        public static IEnumerable<Local> GetUploadDatas<Local>()
            where Local : LocalEntity.BaseEntity, ICanUploadEntity
        {
            BaseLocalService<Local>.IsForcedExpired = true;
            var localRepository = BaseLocalService<Local>.CurrentRepository;
            return localRepository.FindList(o => o.IsUpload).ToList();
        }

        public static IEnumerable<Local> GetUpdateDatas<Local>()
             where Local : LocalEntity.BaseEntity, ICanUpdateEntity
        {
            BaseLocalService<Local>.IsForcedExpired = true;
            var localRepository = BaseLocalService<Local>.CurrentRepository;
            return localRepository.FindList(o => o.HasUpdate).ToList();
        }

        /// <summary>
        /// 从服务器更新数据
        /// </summary>
        public static void UpdateFromServer(UpdateFormData o, Action callback)
        {
            try
            {
                SqliteTrap.PushAction<UpdateFormData>((o1) =>
                {
                    var context = ContextFactory.GetCurrentContext<SqliteDbContext>();

                    using (var localTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var datas = o1.Datas;
                            foreach (var item in datas)
                            {
                                var entity = LocalDataSyncContext.Entities[item.Key];
                                var jsons = JsonConvert.SerializeObject(item.Value);
                                Save(entity, jsons);
                            }
                            localTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            localTransaction.Rollback();

                        }
                        finally
                        {
                            ContextFactory.GetCurrentContext<SqliteDbContext>(true);
                            context.Database.Connection.Close();
                            context.Database.Connection.Dispose();

                        }
                    }
                }, callback, o);

            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 保存数据到实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体实例</param>
        /// <param name="datas">json实体集合</param>
        private static void Save<TEntity>(TEntity entity, string datas)
        where TEntity : LocalEntity.BaseEntity, new()
        {
            var serverSources = JsonConvert.DeserializeObject<List<TEntity>>(datas).ToList();
            var localRepository = BaseLocalService<TEntity>.CurrentRepository;
            var oldList = localRepository.FindList(o => true).ToList();
            if ((entity as ICanUploadEntity) != null)
            {
                oldList = oldList.Where(o => !(o as ICanUploadEntity).IsUpload).ToList();
            }
            if ((entity as ICanUpdateEntity) != null)
            {
                oldList = oldList.Where(o => !(o as ICanUpdateEntity).HasUpdate).ToList();
            }
            //delete
            if (oldList.Count > 0)
                localRepository.RemoveRange(oldList);
            //add
            if (serverSources.Count > 0)
                localRepository.AddRange(serverSources);
        }

        /// <summary>
        /// 更新实体上传状态，删除失效数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体实例</param>
        /// <param name="datas">实体集合</param>
        /// <param name="SaveMonth">保存时效</param>
        public static void UploadStatus<TEntity>(TEntity entity, IEnumerable<object> datas, int SaveMonth)
        where TEntity : LocalEntity.BaseEntity, ICanUploadEntity, new()
        {
            var localRepository = BaseLocalService<TEntity>.CurrentRepository;

            var tempsDatas = (datas as IEnumerable<TEntity>).ToList();
            var saveDatas = localRepository.FindList(o => true).ToList().Where(o => tempsDatas.Exists(p => p.EqualsEntityPrimaryKey(o))).ToList();
            foreach (var item in saveDatas)
            {
                item.IsUpload = false;
            }
            bool isSuccess = localRepository.Update(saveDatas);
            //   var dt = DateTime.Now.AddMonths(-SaveMonth);
            //var ranges = localRepository.FindList(p => p.CreateDT < dt && p.IsUpload).ToList();
            //localRepository.RemoveRange(ranges);
        }

        /// <summary>
        /// 更新实体状态
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体实例</param>
        /// <param name="datas">实体集合</param>
        /// <param name="SaveMonth">保存时效</param>
        public static void UpdateStatus<TEntity>(TEntity entity, IEnumerable<object> datas)
        where TEntity : LocalEntity.BaseEntity, ICanUpdateEntity, new()
        {
            var localRepository = BaseLocalService<TEntity>.CurrentRepository;
            var tempsDatas = (datas as IEnumerable<TEntity>).ToList();

            var saveDatas = localRepository.FindList(o => true).ToList().Where(o => tempsDatas.Exists(p => p.EqualsEntityPrimaryKey(o))).ToList();
            foreach (var item in saveDatas)
            {
                item.HasUpdate = false;
            }
            bool isSuccess = localRepository.Update(saveDatas);

        }


        /// <summary>
        /// 更新多表上传状态，删除失效数据
        /// </summary>
        /// <param name="info">更新信息</param>
        /// <param name="SaveMonth">保存时效，默认值：0（月）</param>
        public static void Upload(UpdateFormData info, Action callback, int SaveMonth = 0)
        {

            try
            {
                SqliteTrap.PushAction<UploadParams>((o) =>
                {
                    var context = ContextFactory.GetCurrentContext<SqliteDbContext>();
                    using (var localTransaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var datas = o.Data.Datas;
                            foreach (var item in datas)
                            {
                                var entity = LocalDataSyncContext.Entities[item.Key];
                                if (o.Data.Mode == DataSyncMode.UploadToServer)
                                {
                                    if (item.Value.Count() > 0)
                                        UploadStatus(entity, item.Value, o.SaveMonth);
                                }
                                else if (o.Data.Mode == DataSyncMode.UpdateToServer)
                                {
                                    UpdateStatus(entity, item.Value);
                                }
                            }
                            localTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            localTransaction.Rollback();
                        }
                    }
                }, callback, new UploadParams() { Data = info, SaveMonth = SaveMonth });
            }
            catch (Exception)
            {

            }
        }
    }

    public class UploadParams
    {
        public UpdateFormData Data { get; set; }

        public int SaveMonth { get; set; }

    }
}
