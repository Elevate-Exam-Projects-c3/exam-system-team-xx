using exam_system.Features.Attempts.StartAttempt.DTOs;
using exam_system.Features.Attempts.StartAttempt.ViewModels;
using Mapster;

namespace exam_system.Features.Attempts.StartAttempt.MappingConfigs;

public sealed class StartAttemptMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<StartAttemptQuestionResponse, StartAttemptQuestionViewModel>();

        config.NewConfig<StartAttemptQuizResponse,  StartAttemptQuizViewModel>()
            .AfterMapping((src,dest) =>
            {
                dest.StudentQuizRemainingAttempts = src.QuizMaxAttempts - src.StudentQuizAttempts;
            });
    }
}
