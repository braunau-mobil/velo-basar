#warning    @todo
//using BraunauMobil.VeloBasar.BusinessLogic;
//using BraunauMobil.VeloBasar.Controllers;
//using BraunauMobil.VeloBasar.Parameters;
//using BraunauMobil.VeloBasar.Routing.Base;
//using BraunauMobil.VeloBasar.Tests.Controllers.CrudControllerTests;
//using Xan.AspNetCore.Mvc.Crud;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.BasarsControllerTests;

//public class Create
//	: AbstractCreate<BasarModel, ListParameter>
//{
//    protected override CrudControllerOLD<BasarModel, ListParameter> CreateSut(ICrudContext<BasarModel> context, ICrudRouterOLD<BasarModel> router)
//    {
//		ArgumentNullException.ThrowIfNull(context);
//		ArgumentNullException.ThrowIfNull(router);

//        var basarContext = new BasarContextWrapper(context);
//        var basarRouter = new BasarRouterWrapper(router);

//		return new BasarsController(basarRouter, basarContext, new Mock<IStatisticContext>().Object, Mockups.Txt);
//    }
//}

