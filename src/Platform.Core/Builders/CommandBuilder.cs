using System.CommandLine;

namespace Platform.Core.Builders
{
    public static class CommandBuilder
    {
        public static Command Create(
            string name,
            string desc,
            IEnumerable<Argument>? arguments = null,
            IEnumerable<Option>? options = null)
        {
            Command command = new(name, desc);

            if (arguments is not null)
            {
                foreach (Argument argument in arguments)
                {
                    command.Add(argument);
                }
            }

            if (options is not null)
            {
                foreach (Option option in options)
                {
                    command.Add(option);
                }
            }

            return command;
        }
    }
}
