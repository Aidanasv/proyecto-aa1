namespace Views;

using Models;

using Spectre.Console;

public class MainMenu
{
    private UserMenu userMenu = new();
    public void Show()
    {
        var title = new FigletText("Música")
            .Centered()
            .Color(Color.Green);

        AnsiConsole.Write(title);

        bool isEnd = true;

        var opcions = new Dictionary<int, string>
            {
                { 1, "🎧 Iniciar sesión" },
                { 2, "📝 Registrarse" },
                { 3, "🌐 Zona pública" },
                { 4, "❌ Salir"}
            };

        while (isEnd)
        {

            int opcion = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("[bold underline green]Menú Principal[/]")
                    .MoreChoicesText("[grey](Mueve de arriba hacia abajo para seleccionar tu opción)[/]")
                    .AddChoices(opcions.Keys)
                    .UseConverter(choice => $"{choice}- {opcions[choice]}"));

            switch (opcion)
            {
                case 1:
                    userMenu.Login();
                    break;
                case 2:
                    userMenu.Register();
                    break;
                case 3:
                    break;
                case 4:
                    isEnd = false;
                    break;
            }
        }
    }


}

