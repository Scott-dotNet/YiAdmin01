## READ ME

### 启动前配置

**修改数据库连接**
编辑 `YiAdmin01.Web` 目录下的`appsetting.json`文件，设置如下：

```json
{
    "DBProvider": "SqlServer",
	"DBConnectionString": 	"server=localhost;database=YiAdmin01;user=sa;password=sqlserver123;TrustServerCertificate=true;"
}
```

其中：服务器、数据库、用户、密码修改为你自己的。

**配置数据库**

在`SqlServer`中创建`YiAdmin01`数据库，执行根目录下`Script`脚本。

**启动系统**

+ 打开项目，打开Git，选分支`B2-login`
+ 设置启动项 `YiAdmin01.Web`，按 F5 键运行
+ 若无网页弹出，则需要打开浏览器，输入: [http://localhost:5000](http://localhost:5000/) （默认账户 admin/123456）