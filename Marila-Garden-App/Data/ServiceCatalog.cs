using Marila_Garden_App.Models;

namespace Marila_Garden_App.Data;

public static class ServiceCatalog
{
    private static readonly IReadOnlyList<ServiceInfo> _services =
        new List<ServiceInfo>
        {
            CreateGardenDesignService(),
            CreateMaintenanceService(),
            CreatePlantingService(),
            CreatePruningService()
        };

    public static IReadOnlyList<ServiceInfo> GetAll()
    {
        return _services;
    }

    public static ServiceInfo? GetById(string serviceId)
    {
        if (string.IsNullOrWhiteSpace(serviceId))
            return null;

        return _services.FirstOrDefault(service =>
            string.Equals(
                service.Id,
                serviceId,
                StringComparison.OrdinalIgnoreCase));
    }

    private static ServiceInfo CreateGardenDesignService()
    {
        return new ServiceInfo
        {
            Id = "garden-design",
            Name = "Diseño de jardín",
            Icon = "desing_service_icon_app.png",
            ShortDescription =
                "Espacios naturales personalizados",

            Description =
                "Diseñamos jardines adaptados al estilo de cada espacio, " +
                "las condiciones del entorno y las necesidades del cliente. " +
                "Cada propuesta busca integrar naturaleza, estética y funcionalidad.",

            EstimatedDuration =
                "La duración depende del tamaño y la complejidad del proyecto.",

            CoverImage = "design_garden_01.jpg",

            Includes = new[]
            {
                "Evaluación inicial del espacio.",
                "Conceptualización y distribución del jardín.",
                "Selección de plantas y materiales.",
                "Propuesta adaptada al entorno y al estilo del cliente."
            },

            Benefits = new[]
            {
                "Mejora la estética y el valor del espacio.",
                "Optimiza la distribución de las áreas verdes.",
                "Facilita el mantenimiento futuro.",
                "Crea ambientes naturales, cómodos y funcionales."
            },

            Images = new[]
            {
                "design_garden_01.jpg",
                "design_garden_02.jpg",
                "design_garden_03.jpg",
                "design_garden_04.jpg"
            }
        };
    }

    private static ServiceInfo CreateMaintenanceService()
    {
        return new ServiceInfo
        {
            Id = "maintenance",
            Name = "Mantenimiento",
            Icon = "mantenimiento_service_icon_app.png",
            ShortDescription =
                "Cuidado responsable y continuo",

            Description =
                "Ofrecemos mantenimiento periódico para preservar la salud, " +
                "el orden y la apariencia del jardín. El servicio se adapta " +
                "a las características de las plantas y del espacio.",

            EstimatedDuration =
                "Puede contratarse como servicio puntual o mantenimiento periódico.",

            CoverImage = "maintenance_01.jpg",

            Includes = new[]
            {
                "Limpieza general de las áreas verdes.",
                "Control y retiro de malezas.",
                "Revisión del estado de las plantas.",
                "Adecuación estética del jardín."
            },

            Benefits = new[]
            {
                "Mantiene el jardín saludable y organizado.",
                "Previene el deterioro de las plantas.",
                "Permite detectar problemas oportunamente.",
                "Conserva la imagen del espacio durante todo el año."
            },

            Images = new[]
            {
                "maintenance_01.jpg",
                "maintenance_02.jpg",
                "maintenance_03.jpg",
                "maintenance_04.jpg"
            }
        };
    }

    private static ServiceInfo CreatePlantingService()
    {
        return new ServiceInfo
        {
            Id = "planting",
            Name = "Plantación",
            Icon = "plantacion_service_icon_app.png",
            ShortDescription =
                "Plantas, maceteros y detalles",

            Description =
                "Realizamos la plantación de especies ornamentales y naturales, " +
                "considerando el clima, la iluminación, el suelo y las condiciones " +
                "particulares de cada espacio.",

            EstimatedDuration =
                "La duración varía según la cantidad y el tipo de especies.",

            CoverImage = "planting_01.jpg",

            Includes = new[]
            {
                "Evaluación de las condiciones del espacio.",
                "Selección de especies adecuadas.",
                "Preparación del área de plantación.",
                "Orientación para el cuidado inicial."
            },

            Benefits = new[]
            {
                "Favorece el desarrollo saludable de las plantas.",
                "Reduce el riesgo de utilizar especies inadecuadas.",
                "Mejora la composición visual del jardín.",
                "Permite aprovechar mejor las condiciones naturales del espacio."
            },

            Images = new[]
            {
                "planting_01.jpg",
                "planting_02.jpg",
                "planting_03.jpg",
                "planting_04.jpg"
            }
        };
    }

    private static ServiceInfo CreatePruningService()
    {
        return new ServiceInfo
        {
            Id = "pruning",
            Name = "Poda profesional",
            Icon = "poda_service_icon_app.png",
            ShortDescription =
                "Control, salud y estética vegetal",

            Description =
                "Aplicamos técnicas de poda según el tipo de planta y su estado, " +
                "retirando ramas innecesarias y favoreciendo un crecimiento más " +
                "equilibrado y saludable.",

            EstimatedDuration =
                "La duración depende de la cantidad, el tamaño y el estado de las plantas.",

            CoverImage = "pruning_01.jpg",

            Includes = new[]
            {
                "Evaluación previa de las plantas.",
                "Retiro de ramas secas o deterioradas.",
                "Poda de formación y control.",
                "Limpieza del área intervenida."
            },

            Benefits = new[]
            {
                "Estimula un crecimiento más saludable.",
                "Mejora la forma y apariencia de las plantas.",
                "Reduce riesgos provocados por ramas deterioradas.",
                "Favorece la entrada de luz y aire."
            },

            Images = new[]
            {
                "pruning_01.jpg",
                "pruning_02.jpg",
                "pruning_03.jpg",
                "pruning_04.jpg"
            }
        };
    }
}