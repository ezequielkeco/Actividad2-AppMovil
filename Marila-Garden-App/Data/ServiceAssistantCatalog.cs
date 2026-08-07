using Marila_Garden_App.Models.Assistant;

namespace Marila_Garden_App.Data;

public static class ServiceAssistantCatalog
{
    private static readonly IReadOnlyList<AssistantQuestion> _questions =
        new List<AssistantQuestion>
        {
            CreateMainNeedQuestion(),
            CreateCurrentSituationQuestion(),
            CreatePriorityQuestion(),
            CreateSpaceTypeQuestion()
        };

    public static IReadOnlyList<AssistantQuestion> GetAll()
    {
        return _questions;
    }

    private static AssistantQuestion CreateMainNeedQuestion()
    {
        return new AssistantQuestion
        {
            Id = "main-need",
            Title = "¿Qué necesitas mejorar?",
            Description =
                "Selecciona la opción que mejor describa lo que deseas lograr.",

            Options = new[]
            {
                new AssistantOption
                {
                    Id = "transform-space",
                    Title = "Transformar o diseñar un espacio verde",
                    Description =
                        "Quiero mejorar la distribución, estética o concepto general del jardín.",
                    Icon = "assistant_design.png",

                    Scores = new Dictionary<string, int>
                    {
                        ["garden-design"] = 4,
                        ["planting"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "maintain-garden",
                    Title = "Mantener mi jardín saludable y organizado",
                    Description =
                        "Necesito cuidado periódico, limpieza y conservación.",
                    Icon = "assistant_maintenance.png",

                    Scores = new Dictionary<string, int>
                    {
                        ["maintenance"] = 4,
                        ["pruning"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "renew-plants",
                    Title = "Incorporar o renovar plantas",
                    Description =
                        "Quiero agregar nuevas especies o sustituir plantas existentes.",
                    Icon = "assistant_planting.png",

                    Scores = new Dictionary<string, int>
                    {
                        ["planting"] = 4,
                        ["garden-design"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "control-growth",
                    Title = "Podar o controlar el crecimiento",
                    Description =
                        "Tengo plantas o árboles que necesitan poda o control.",
                    Icon = "assistant_pruning.png",

                    Scores = new Dictionary<string, int>
                    {
                        ["pruning"] = 4,
                        ["maintenance"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "not-sure",
                    Title = "No estoy seguro",
                    Description =
                        "Necesito orientación para identificar el servicio adecuado.",
                    Icon = "assistant_help.png",

                    Scores = new Dictionary<string, int>()
                }
            }
        };
    }

    private static AssistantQuestion CreateCurrentSituationQuestion()
    {
        return new AssistantQuestion
        {
            Id = "current-situation",
            Title = "¿Cómo describirías actualmente tu espacio?",
            Description =
                "Esto nos ayudará a comprender mejor el estado actual de tu jardín.",

            Options = new[]
            {
                new AssistantOption
                {
                    Id = "needs-transformation",
                    Title = "Quiero crear o transformar el jardín",
                    Scores = new Dictionary<string, int>
                    {
                        ["garden-design"] = 3,
                        ["planting"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "needs-care",
                    Title = "Ya tengo un jardín y necesita cuidados",
                    Scores = new Dictionary<string, int>
                    {
                        ["maintenance"] = 3,
                        ["pruning"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "needs-plants",
                    Title = "Quiero agregar nuevas plantas",
                    Scores = new Dictionary<string, int>
                    {
                        ["planting"] = 3,
                        ["garden-design"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "needs-pruning",
                    Title = "Hay plantas o árboles que necesitan poda",
                    Scores = new Dictionary<string, int>
                    {
                        ["pruning"] = 3,
                        ["maintenance"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "multiple-improvements",
                    Title = "El espacio necesita varias mejoras",
                    Scores = new Dictionary<string, int>
                    {
                        ["garden-design"] = 2,
                        ["maintenance"] = 2,
                        ["planting"] = 1,
                        ["pruning"] = 1
                    }
                }
            }
        };
    }

    private static AssistantQuestion CreatePriorityQuestion()
    {
        return new AssistantQuestion
        {
            Id = "priority",
            Title = "¿Qué resultado es más importante para ti?",
            Description =
                "Selecciona la prioridad principal para tu espacio.",

            Options = new[]
            {
                new AssistantOption
                {
                    Id = "improve-appearance",
                    Title = "Mejorar la apariencia del espacio",
                    Scores = new Dictionary<string, int>
                    {
                        ["garden-design"] = 3,
                        ["planting"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "keep-healthy",
                    Title = "Mantener las plantas saludables",
                    Scores = new Dictionary<string, int>
                    {
                        ["maintenance"] = 3,
                        ["pruning"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "add-species",
                    Title = "Incorporar nuevas especies",
                    Scores = new Dictionary<string, int>
                    {
                        ["planting"] = 3
                    }
                },

                new AssistantOption
                {
                    Id = "control-growth",
                    Title = "Controlar el crecimiento de plantas o árboles",
                    Scores = new Dictionary<string, int>
                    {
                        ["pruning"] = 3,
                        ["maintenance"] = 1
                    }
                },

                new AssistantOption
                {
                    Id = "general-improvement",
                    Title = "Conseguir una mejora general",
                    Scores = new Dictionary<string, int>
                    {
                        ["garden-design"] = 2,
                        ["maintenance"] = 2,
                        ["planting"] = 1,
                        ["pruning"] = 1
                    }
                }
            }
        };
    }

    private static AssistantQuestion CreateSpaceTypeQuestion()
    {
        return new AssistantQuestion
        {
            Id = "space-type",
            Title = "¿Dónde necesitas el servicio?",
            Description =
                "Esta información nos ayudará a contextualizar mejor la recomendación.",

            Options = new[]
            {
                new AssistantOption
                {
                    Id = "home",
                    Title = "Vivienda",
                    Scores = new Dictionary<string, int>()
                },

                new AssistantOption
                {
                    Id = "villa",
                    Title = "Villa",
                    Scores = new Dictionary<string, int>()
                },

                new AssistantOption
                {
                    Id = "business",
                    Title = "Negocio",
                    Scores = new Dictionary<string, int>()
                },

                new AssistantOption
                {
                    Id = "other",
                    Title = "Otro espacio",
                    Scores = new Dictionary<string, int>()
                }
            }
        };
    }
}