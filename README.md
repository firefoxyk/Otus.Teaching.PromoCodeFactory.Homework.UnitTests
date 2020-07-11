# Otus.Teaching.PromoCodeFactory

Проект для домашних заданий и демо по курсу `C# ASP.NET Core Разработчик` от `Отус`.
Cистема `Promocode Factory` для выдачи промокодов партнеров для клиентов по группам предпочтений.

Данный проект является стартовой точкой для Homework №4

Подробное описание проекта можно найти в [Wiki](https://gitlab.com/devgrav/otus.teaching.promocodefactory/-/wikis/Home)

Описание домашнего задания в [Homework Wiki](https://gitlab.com/devgrav/otus.teaching.promocodefactory/-/wikis/Homework-4)

Для запуска проекта с базой данных `PostgreSQL` нужно использовать `docker-compose up promocode-factory-db` из корня проекта, в `Startup` файле проекта `WebHost` должна быть установлена настройка `UseNpgsql`