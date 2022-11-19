using Xan.Extensions;

namespace BraunauMobil.VeloBasar.Tests;

public class Mockups
{
    public static Mock<IClock> Clock(DateTime now)
    {
        Mock<IClock> mockClock = new ();
        mockClock.Setup(m => m.GetCurrentDateTime())
            .Returns(now);
        return mockClock;
    }

    //private static readonly VeloFixture _fixture = new();

    //public static VeloTexts Txt { get; } = new(new Mock<IStringLocalizer<SharedResource>>().Object);

    //public static IVeloRouter Router(Action<Mock<IVeloRouter>> setup)
    //{
    //    var mockRouter = new Mock<IVeloRouter>();
    //    setup(mockRouter);
    //    return mockRouter.Object;
    //}

    //public static Mock<TValidator> ValidatorFalse<TValidator, TModel>(TModel model)
    //    where TValidator : class, IValidator<TModel>
    //{
    //    var mockValidator = new Mock<TValidator>();
    //    mockValidator.Setup(v => v.Validate(model))
    //        .Returns(new ValidationResult(_fixture.CreateMany<ValidationFailure>()));

    //    return mockValidator;
    //}

    //public static Mock<TValidator> ValidatorTrue<TValidator, TModel>(TModel model)
    //    where TValidator : class, IValidator<TModel>
    //{
    //    var mockValidator = new Mock<TValidator>();
    //    mockValidator.Setup(v => v.Validate(model))
    //        .Returns(new ValidationResult());

    //    return mockValidator;
    //}
}
