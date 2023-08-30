/// <summary>
/// SqlSugar加载函数
/// </summary>
public static class StaticSqlSugarHelper
{
    #region 加载

    public class dbConn
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Conn { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Type { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public bool useAutoSyncStructure { set; get; } = false;
    }

    /// <summary>
    /// 获取数据库类型
    /// </summary>
    /// <param name="_val"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static SqlSugar.DbType GetSqlSugarDbType(this string _val)
    { 
        switch (_val.ToUpper())
        {
            default:
                throw new Exception($"未支持的数据库类型 {_val}");
            case "SQLITE":
                return SqlSugar.DbType.Sqlite;
            case "DM":
                return SqlSugar.DbType.Dm;
            case "SQLSERVER":
                return SqlSugar.DbType.SqlServer;
            case "MYSQL":
                return SqlSugar.DbType.MySql;
            case "ORACLE":
                return SqlSugar.DbType.Oracle;
        }    
    }

    /// <summary>
    /// 输出委托
    /// 委托所指向的函数必须跟委托具有相同的签名（函数的返回值和参数）
    /// </summary>
    /// <param name="_log"></param>
    public delegate void DbAuditOutPut(StringBuilder _log, string _type);

    /// <summary>
    /// 多租户加载
    /// </summary>
    /// <param name="services"></param>
    /// <param name="auditAction"></param>
    /// <param name="ElapsedAction"></param>
    /// <param name="elapsedSeconds"></param>
    public static void SaasSetup(this IServiceCollection services, DbAuditOutPut? auditAction = null, DbAuditOutPut? ElapsedAction = null, long elapsedSeconds = 0)
    {
        //加载默认数据库
        var DefConfigConnection = new ConnectionConfig()
        {
            ConfigId = "System:Db:Code".GetConfigVal(),
            DbType = "System:Db:DbType".GetConfigVal().GetSqlSugarDbType(),
            ConnectionString = "System:Db:Conn".GetConfigVal(),
            IsAutoCloseConnection = true,
            ConfigureExternalServices = new ConfigureExternalServices()
            {
                EntityNameService = (type, entity) =>
                {
                    //禁止删除列
                    entity.IsDisabledDelete = true;
                }
            },
            MoreSettings = new ConnMoreSettings()
            {
                //自动转成大写表
                IsAutoToUpper = true
            }
        };
        using SqlSugarScope DefSqlSugar = new SqlSugarScope(DefConfigConnection);
        //CodeFirst创建所有表
        if ("System:Db:DbType".GetConfigVal().GetSqlSugarDbType() == SqlSugar.DbType.Sqlite) DefSqlSugar.DbMaintenance.CreateDatabase();


        //Assembly assembly = Assembly.LoadFrom(($"{AppContext.BaseDirectory}\\Mazinger.ForDataStatistics.dll").GetRuntimeDirectory());
        //IEnumerable<Type> typelist = assembly.GetTypes().Where(c => c.Namespace == "Mazinger.ForDataStatistics.Models.CodeFist");

        IEnumerable<Type> typelist = typeof(UserEntity).Assembly.GetTypes().Where(c => c.Namespace == "Mazinger.Models.CodeFist"); //命名空间过滤，当然你也可以写其他条件过滤 ;

        DefSqlSugar.CodeFirst.InitTables(typelist.ToArray());
        //创建默认用户数据
        if (!string.IsNullOrEmpty("System:Db:MasterUserCode".GetConfigVal()))
        {
            if (!DefSqlSugar.Queryable<UserEntity>().Where(_a => _a.userCode == "System:Db:MasterUserCode".GetConfigVal()).Any())
            {
                DefSqlSugar.Insertable(new UserEntity { 
                 userCode = "System:Db:MasterUserCode".GetConfigVal(),
                 userName = "System:Db:MasterUserName".GetConfigVal(),
                 userPwd = "System:Db:MasterUserPwdMd5".GetConfigVal(),
                }).ExecuteCommand();

                DefSqlSugar.Insertable(new UserPermissionEntity { permissionName = "管理", permissionType = "1", permissionUser = "System:Db:MasterUserCode".GetConfigVal() }).ExecuteCommand();
            }
        }
        //创建默认模块链接
        if (!DefSqlSugar.Queryable<ModuleUrlEntity>().Any())
        {
            var _moduleList = new List<ModuleUrlEntity> { 
                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 99, Wml_name = "系统管理", Wml_purview ="管理", Wml_icon="mdi-cog", Wml_pid =0, Wml_target=0  },
                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 99.1M, Wml_name = "用户管理", Wml_icon="mdi-account-group", Wml_url="/pages/controls/users", Wml_pid =99, Wml_target=0  },
                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 99.2M, Wml_name = "模块管理", Wml_icon="mdi-menu-open", Wml_url="/pages/controls/modules", Wml_pid =99, Wml_target=0  },
                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 99.3M, Wml_name = "数据库", Wml_icon="mdi-database-cog", Wml_url="/pages/controls/dbs", Wml_pid =99, Wml_target=0  },
                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 99.4M, Wml_name = "查询函数", Wml_icon="mdi-database-search", Wml_url="/pages/controls/queryfunc", Wml_pid =99, Wml_target=0  },

                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 100, Wml_name = "基础表", Wml_purview ="管理", Wml_icon="mdi-book-cog-outline", Wml_pid =0, Wml_target=0  },
                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 100.1M, Wml_name = "单位管理", Wml_icon="mdi-domain", Wml_url="/pages/controls/company", Wml_pid =100, Wml_target=0  },
                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 100.2M, Wml_name = "部门管理", Wml_icon="mdi-door-sliding", Wml_url="/pages/controls/dept", Wml_pid =100, Wml_target=0  },
                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 100.3M, Wml_name = "职务管理", Wml_icon="mdi-badge-account-alert-outline", Wml_url="/pages/controls/duty", Wml_pid =100, Wml_target=0  },
                new ModuleUrlEntity{  Wml_guid =Guid.NewGuid().ToString(), Wml_id = 100.4M, Wml_name = "职称管理", Wml_icon="mdi-table-account", Wml_url="/pages/controls/tech", Wml_pid =100, Wml_target=0  },

            };

            DefSqlSugar.Insertable(_moduleList).ExecuteCommand();
        }

        var dbList = DefSqlSugar.Queryable<DbConnEntity>().OrderBy(_a => _a.dbCode).ToList();
        var configConnection = dbList.Select(db => new ConnectionConfig
        {
            ConfigId = db.dbCode,
            DbType = db.dbType.GetSqlSugarDbType(),
            ConnectionString = db.dbConn,
            IsAutoCloseConnection = true,
            ConfigureExternalServices = new ConfigureExternalServices()
            {
                EntityNameService = (type, entity) =>
                {
                    //禁止删除列
                    entity.IsDisabledDelete = true;
                }
            },
            MoreSettings = new ConnMoreSettings()
            {
                //自动转成大写表
                IsAutoToUpper = true
            }
        }).ToList();

        configConnection.Add(DefConfigConnection);
        SqlSugarScope sqlSugar = new SqlSugarScope(configConnection,
           db =>
           {
               //SQL执行完
               db.Aop.OnLogExecuted = (sql, pars) =>
               {
                   StringBuilder _log = new StringBuilder();
                   //审计日志
                   if (auditAction != null)
                   {
                       _log.AppendLine($"执行时间：{db.Ado.SqlExecutionTime.TotalSeconds}秒");
                       _log.AppendLine($"语句：{sql}");
                       _log.AppendLine($"参数：{pars.toJsonStr()}");
                       auditAction.Invoke(_log, "OnLogExecuted");
                   }
                   //性能分析
                   if (ElapsedAction != null && elapsedSeconds > 0 && db.Ado.SqlExecutionTime.TotalSeconds > (double)elapsedSeconds)
                   {
                       _log.AppendLine($"代码CS文件名：{db.Ado.SqlStackTrace.FirstFileName}");
                       _log.AppendLine($"代码行数：{db.Ado.SqlStackTrace.FirstLine}");
                       _log.AppendLine($"方法名：{db.Ado.SqlStackTrace.FirstMethodName}");
                       ElapsedAction.Invoke(_log, "Elapsed");
                   }
                   //执行完了可以输出SQL执行时间 (OnLogExecutedDelegate) 
                   //Console.Write("time:" + db.Ado.SqlExecutionTime.ToString());
               };
               //SQL执行前
               db.Aop.OnLogExecuting = (sql, pars) =>
               {
                   //5.0.8.2 获取无参数化 SQL 
                   //UtilMethods.GetSqlString(DbType.SqlServer,sql,pars)
                   //StringBuilder _log = new StringBuilder();
                   //_log.AppendLine($"{sql}");
                   //_log.AppendLine($"{pars.toJsonStr()}");
                   ////审计日志
                   //if (auditAction != null) auditAction.Invoke(_log, "OnLogExecuting");
               };
               //SQL报错
               db.Aop.OnError = (exp) =>
               {
                   //exp.sql 这样可以拿到错误SQL，性能无影响拿到ORM带参数使用的SQL
                   //5.0.8.2 获取无参数化 SQL  对性能有影响，特别大的SQL参数多的，调试使用
                   //UtilMethods.GetSqlString(DbType.SqlServer,exp.sql,exp.parameters)           
               };
           });

        services.AddSingleton<ISqlSugarClient>(sqlSugar);

    }

    #endregion

    #region 调用

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="_db"></param>
    /// <param name="_dbName"></param>
    /// <returns></returns>
    public static SqlSugarProvider GetDb(this ISqlSugarClient db, string _dbName)
    {
        if (!db.AsTenant().IsAnyConnection(_dbName)) throw new Exception($"数据库链接不存在");
        return db.AsTenant().GetConnection(_dbName);
    }

    ///// <summary>
    ///// 执行事务(多租户事务，弃用)
    ///// </summary>
    ///// <param name="db"></param>
    ///// <param name="action"></param>
    ///// <returns></returns>
    //public static Task GoTransaction(this ISqlSugarClient db, Action action)
    //{
    //    try {
    //        db.AsTenant().BeginTran();

    //        action();

    //        db.AsTenant().CommitTran();
    //    }
    //    catch(Exception ex) {
    //        db.AsTenant().RollbackTran();
    //        throw ex;
    //    }

    //    return Task.CompletedTask;
    //}


    /// <summary>
    /// 执行事务
    /// </summary>
    /// <param name="db"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Task GoTransaction(this SqlSugarProvider db, Action action)
    {
        try
        {
            db.Ado.BeginTran();

            action();

            db.Ado.CommitTran();
        }
        catch (Exception ex)
        {
            db.Ado.RollbackTran();
            throw ex;
        }

        return Task.CompletedTask;
    }
    #endregion
}