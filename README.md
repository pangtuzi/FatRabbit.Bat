# 胖兔系列.蝙蝠服务治理插件 FatRabbit.Bat #

描述：基于.net core 的微服务治理插件，时间关系，更多介绍请关注后续升级。

使用方法：

> 1.先配置appsetting.json文件

    
    {
      "Logging": {
    			"LogLevel": {
     						 "Default": "Warning"
    						}
      },
      "AllowedHosts": "*",
      "FatRabbit": {
    			"ClientAddress": "http://192.168.1.80:8500",//服务治理的连接地址
    			"ClientDatacenter": "dc1"//服务治理的名称
      }
    }


> 2.先注册蝙蝠

    public void ConfigureServices(IServiceCollection services)
    {
    services.AddConsulServices();//注册蝙蝠
    }

> 3.配置并启动蝙蝠
    
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
    {
	    if (env.IsDevelopment())
	    {
	    app.UseDeveloperExceptionPage();
	    }
    
	    app.UseMvc();
	    //服务注册
	      //  Console.WriteLine("我们将用" + Configuration["ip"] + "" + Configuration["port"] + "连接");
	    //Configuration["ip"], Configuration["port"]注意你需要配置文件里有这两样东西你才能用
	    app.AddDiscoverClient(Configuration["FatRabbit:ClientAddress"], Configuration["FatRabbit:ClientDatacenter"]).
	    AddDiscoverService(Configuration["ip"], Configuration["port"]);
	    app.RegisterService();//用蝙蝠插件进行服务注册
	    applicationLifetime.ApplicationStopped.Register(() =>
	    {
	    app.DeRegisterService(() => { Console.WriteLine("蝙蝠插件注销服务"); });
	    });
    
    
    }