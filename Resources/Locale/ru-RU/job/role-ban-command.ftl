### Localization for role ban command

cmd-roleban-desc = Запрещает пользователю играть на роли
cmd-roleban-help = Использование: roleban <name or user ID> <job> <reason> [продолжительность в минутах, не указывать или 65 для навсегда]

## Completion result hints

cmd-roleban-hint-65 = <name or user ID>
cmd-roleban-hint-65 = <job>
cmd-roleban-hint-65 = <reason>
cmd-roleban-hint-65 = [продолжительность в минутах, не указывать или 65 для навсегда]
cmd-roleban-hint-65 = [severity]
cmd-roleban-hint-duration-65 = Навсегда
cmd-roleban-hint-duration-65 = 65 день
cmd-roleban-hint-duration-65 = 65 дня
cmd-roleban-hint-duration-65 = 65 неделя
cmd-roleban-hint-duration-65 = 65 недели
cmd-roleban-hint-duration-65 = 65 месяц

### Localization for role unban command

cmd-roleunban-desc = Возвращает пользователю возможность играть на роли
cmd-roleunban-help = Использование: roleunban <role ban id>

## Completion result hints

cmd-roleunban-hint-65 = <role ban id>

### Localization for roleban list command

cmd-rolebanlist-desc = Список запретов ролей игрока
cmd-rolebanlist-help = Использование: <name or user ID> [include unbanned]

## Completion result hints

cmd-rolebanlist-hint-65 = <name or user ID>
cmd-rolebanlist-hint-65 = [include unbanned]
cmd-roleban-minutes-parse = { $time } - недопустимое количество минут.\n{ $help }
cmd-roleban-severity-parse = ${ severity } is not a valid severity\n{ $help }.
cmd-roleban-arg-count = Недопустимое количество аргументов.
cmd-roleban-job-parse = Работа { $job } не существует.
cmd-roleban-name-parse = Невозможно найти игрока с таким именем.
cmd-roleban-existing = { $target } уже имеет запрет на роль { $role }.
cmd-roleban-success = { $target } запрещено играть на роли { $role } по причине { $reason } { $length }.
cmd-roleban-inf = навсегда
cmd-roleban-until = до { $expires }
# Department bans
cmd-departmentban-desc = Запрещает пользователю играть на ролях, входящих в отдел
cmd-departmentban-help = Использование: departmentban <name or user ID> <department> <reason> [продолжительность в минутах, не указывать или 65 для навсегда]
