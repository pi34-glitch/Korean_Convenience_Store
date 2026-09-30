namespace KoreanStoreMvc.Helpers
{
    public static class ImagenHelper
    {
        // Diccionario: palabra clave en el nombre -> URL de imagen
        private static readonly Dictionary<string, string> _mapaImagenes = new(StringComparer.OrdinalIgnoreCase)
        {
            // Ramen / Fideos instantáneos
            { "shin ramyun", "https://images.unsplash.com/photo-1569718212165-3a8278d5f624?w=600&q=80" },
            { "shin", "https://images.unsplash.com/photo-1569718212165-3a8278d5f624?w=600&q=80" },
            { "buldak", "https://images.unsplash.com/photo-1591814468924-caf88d1232e1?w=600&q=80" },
            { "samyang", "https://images.unsplash.com/photo-1591814468924-caf88d1232e1?w=600&q=80" },
            { "carbonara", "https://images.unsplash.com/photo-1612929633738-8fe44f7ec841?w=600&q=80" },
            { "jin ramen", "https://images.unsplash.com/photo-1617093727343-374698b1b08d?w=600&q=80" },
            { "ottogi", "https://images.unsplash.com/photo-1617093727343-374698b1b08d?w=600&q=80" },
            { "ramen", "https://images.unsplash.com/photo-1569718212165-3a8278d5f624?w=600&q=80" },
            { "fideo", "https://images.unsplash.com/photo-1552611052-33e04de081de?w=600&q=80" },
            { "udon", "https://images.unsplash.com/photo-1552611052-33e04de081de?w=600&q=80" },
            { "noodle", "https://images.unsplash.com/photo-1552611052-33e04de081de?w=600&q=80" },

            // Bebidas
            { "milkis", "https://images.unsplash.com/photo-1622483767028-3f66f32aef97?w=600&q=80" },
            { "soju", "https://images.unsplash.com/photo-1569529465841-dfecdab7503b?w=600&q=80" },
            { "té verde", "https://images.unsplash.com/photo-1556881286-fc6915169721?w=600&q=80" },
            { "tea", "https://images.unsplash.com/photo-1556881286-fc6915169721?w=600&q=80" },
            { "agua", "https://images.unsplash.com/photo-1523362628745-0c100150b504?w=600&q=80" },
            { "bebida", "https://images.unsplash.com/photo-1622483767028-3f66f32aef97?w=600&q=80" },
            { "coca", "https://images.unsplash.com/photo-1554866585-cd94860890b7?w=600&q=80" },

            // Snacks
            { "pocky", "https://images.unsplash.com/photo-1621939514649-280e6ed8d5e3?w=600&q=80" },
            { "pepero", "https://images.unsplash.com/photo-1621939514649-280e6ed8d5e3?w=600&q=80" },
            { "choco pie", "https://images.unsplash.com/photo-1606312619070-d48b4c652a52?w=600&q=80" },
            { "galleta", "https://images.unsplash.com/photo-1558961363-fa8fdf82db35?w=600&q=80" },
            { "snack", "https://images.unsplash.com/photo-1566478989037-eec170784d0b?w=600&q=80" },
            { "papas", "https://images.unsplash.com/photo-1566478989037-eec170784d0b?w=600&q=80" },
            { "chips", "https://images.unsplash.com/photo-1566478989037-eec170784d0b?w=600&q=80" },

            // Caldos / Sopas
            { "caldo", "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=600&q=80" },
            { "sopa", "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=600&q=80" },

            // Ingredientes por peso
            { "tteokbokki", "https://images.unsplash.com/photo-1635363638580-c2809d049eee?w=600&q=80" },
            { "pastel de arroz", "https://images.unsplash.com/photo-1635363638580-c2809d049eee?w=600&q=80" },
            { "cerdo", "https://images.unsplash.com/photo-1432139555190-58524dae6a55?w=600&q=80" },
            { "pollo", "https://images.unsplash.com/photo-1604503468506-a8da13d82791?w=600&q=80" },
            { "vegetales", "https://images.unsplash.com/photo-1540420773420-3366772f4999?w=600&q=80" },
            { "champiñones", "https://images.unsplash.com/photo-1504545102780-26774c1bb073?w=600&q=80" },
            { "enoki", "https://images.unsplash.com/photo-1504545102780-26774c1bb073?w=600&q=80" },
            { "kimchi", "https://images.unsplash.com/photo-1583224964978-2257b960c3d3?w=600&q=80" },
            { "proteína", "https://images.unsplash.com/photo-1432139555190-58524dae6a55?w=600&q=80" },
        };

        private const string _imagenDefault = "https://images.unsplash.com/photo-1584278860047-22db9ff82bed?w=600&q=80";

        public static string ObtenerImagen(string nombreProducto)
        {
            if (string.IsNullOrWhiteSpace(nombreProducto))
                return _imagenDefault;

            var nombre = nombreProducto.ToLower();

            // Buscar la primera palabra clave que coincida
            foreach (var kvp in _mapaImagenes)
            {
                if (nombre.Contains(kvp.Key.ToLower()))
                    return kvp.Value;
            }

            return _imagenDefault;
        }
    }
}