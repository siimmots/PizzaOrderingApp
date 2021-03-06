# PizzaOrderingApp


**Database migrations**

~~~
dotnet ef migrations add InitialMigration --project DAL --startup-project WebApp
dotnet ef database update  --project DAL --startup-project WebApp
~~~

**Database droping**

~~~
dotnet ef database drop  --project DAL --startup-project WebApp
~~~

## Project images

**Main Menu**

![picture](Images/PizzaMenu.png)


**Create a new Pizza**

![picture](Images/CreatePizza.png)

**Search for pizzas**

![picture](Images/Search.png)

**Ordering**

![picture](Images/OrderPizza.png)

**Pizza statistics**

![picture](Images/PizzaStatistics.png)