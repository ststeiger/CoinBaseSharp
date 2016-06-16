
namespace TestApplication
{


    class LinqReflection
    {


        // public class Person : PersonEx { public string Inexistant; }

        public class Person
        {
            public string Name { get; set; }
            public Person Brother { get; set; }
            public string Email { get; set; }

            public string SnailMail;
            public int Anumber;

            public int? NullableNumber;
        }


        public class T_User
        {
            public System.Guid BE_UID;
            public int BE_ID { get; set; }
            public string BE_Name { get; set; }

            public string EMail;
            public object SnailMail;
            public int Anumber;

            public int? NullableNumber;
        }


        public static void LinqTest()
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();
            ls.Add("foo");
            ls.Add("bar");
            ls.Add("foobar");

            int oobj = 123;
            Person someOne = new Person() { Name = "foo", Email = "foo@bar.com", SnailMail = "Snail" };
            // object inexistant = GetProperty(someOne, "Inexistant");

            object myName = GetProperty(someOne, "Name");
            string myNameString = GetProperty<Person, string>(someOne, "Name");

            int? nullMe = GetProperty<Person, int?>(someOne, "NullableNumber");


            object nullNumObj = GetProperty(someOne, "NullableNumber");
            int? nullNum = GetProperty<Person, int?>(someOne, "NullableNumber");
            System.Console.WriteLine(nullNum);

            SetProperty(someOne, "NullableNumber", null);
            System.Console.WriteLine(someOne);

            SetProperty(someOne, "NullableNumber", -123);
            System.Console.WriteLine(someOne);

            SetProperty(someOne, "NullableNumber", "-123");
            System.Console.WriteLine(someOne);

            SetProperty(someOne, "NullableNumber", System.DBNull.Value);
            System.Console.WriteLine(someOne);


            // object obj = System.DBNull.Value;
            // SetProperty(someOne, "NullableNumber", obj);


            System.Console.WriteLine(myName);
            System.Console.WriteLine(myNameString);
            // SetProperty(someOne, "Anumber", oobj);
            // SetProperty(someOne, "SnailMail", "Turtle Mail");
            // SetProperty(someOne, "Email", "SpamMail");
            T_User ben = new T_User();

            int cnt = GetProperty<System.Collections.Generic.List<string>, int>(ls, "cOuNt");
            object objCount = GetProperty(ls, "cOuNt");
            System.Console.WriteLine(cnt);


            // b15186d6-adb1-4c8a-bbfa-830b24417e8b
            string SQL = @"SELECT 'B15186D6-ADB1-4C8A-BBFA-830B24417E8B' AS BE_UID, '123' AS BE_ID, 'Carbon Unit' AS BE_Name, 'foo@bar.foobar' AS EMail, 'omg' AS SnailMail, CAST(NULL AS integer) AS NullableNumber;";
            // SQL = @"SELECT CAST(NULL AS uniqueidentifier) AS BE_UID"; // Test NULLing non-null type error message...

            using (System.Data.Common.DbDataReader rdr = CoinBaseSharp.SQL.ExecuteReader(SQL))
            {
                do
                {
                    int fieldCount = rdr.FieldCount;
                    System.Type[] ts = new System.Type[fieldCount];
                    string[] fieldNames = new string[fieldCount];
                    System.Action<T_User, object>[] fieldSetters = new System.Action<T_User, object>[fieldCount];

                    for (int i = 0; i < fieldCount; ++i)
                    {
                        ts[i] = rdr.GetFieldType(i);
                        fieldNames[i] = rdr.GetName(i);
                        fieldSetters[i] = GetSetter<T_User>(fieldNames[i]);
                    } // Next i 


                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            for (int i = 0; i < fieldCount; ++i)
                            {
                                object objValue = rdr.GetValue(i);
                                // if (object.ReferenceEquals(objValue, System.DBNull.Value)) objValue = null;

                                System.Console.WriteLine(ts[i]);
                                //int abc = 123;
                                // SetProperty(ben, fieldNames[i], abc);
                                // SetProperty(ben, fieldNames[i], objValue);
                                fieldSetters[i](ben, objValue);
                                System.Console.WriteLine(objValue);
                            } // Next i 

                        } // Whend 

                    } // End if (rdr.HasRows)

                } while (rdr.NextResult());

            } // End Using rdr 


            System.Console.WriteLine(ben.BE_UID);
        } // End Sub LinqTest 



        public static class LambdaExtensions
        {
            // public static void SetPropertyValue<T>(this T target,
            public static void SetPropertyValue<T>(T target,
                System.Linq.Expressions.Expression<System.Func<T, object>> memberLamda, object value)
            {
                var memberSelectorExpression = memberLamda.Body as System.Linq.Expressions.MemberExpression;
                if (memberSelectorExpression != null)
                {
                    var property = memberSelectorExpression.Member as System.Reflection.PropertyInfo;
                    if (property != null)
                    {
                        property.SetValue(target, value, null);
                    }
                }
            }
        }
        // https://stackoverflow.com/questions/8107134/how-set-value-a-property-selector-expressionfunct-tresult
        /// <summary>
        /// Convert a lambda expression for a getter into a setter
        /// </summary>
        public static System.Action<T, TProperty> GetSetter<T, TProperty>(
            System.Linq.Expressions.Expression<System.Func<T, TProperty>> expression)
        {
            var memberExpression = (System.Linq.Expressions.MemberExpression)expression.Body;
            var property = (System.Reflection.PropertyInfo)memberExpression.Member;
            var setMethod = property.GetSetMethod();

            var parameterT = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
            var parameterTProperty = System.Linq.Expressions.Expression.Parameter(typeof(TProperty), "y");

            var newExpression =
                System.Linq.Expressions.Expression.Lambda<System.Action<T, TProperty>>(
                    System.Linq.Expressions.Expression.Call(parameterT, setMethod, parameterTProperty),
                    parameterT,
                    parameterTProperty
                );

            return newExpression.Compile();
        }

        /*
        public static TP CleanProperty<T, TP>(T obj, System.Linq.Expressions.Expression<System.Func<T, TP>> propertySelector) where TP : class
        {
            var valueParam = System.Linq.Expressions.Expression.Parameter(typeof(TP), "value");
            var getValue = propertySelector.Compile();

            // Use the propertySelector body as the left sign of an assign and all the complexity of getting to the property is handled.
            var setValue = System.Linq.Expressions.Expression.Lambda<System.Action<T, TP>>(
                                System.Linq.Expressions.Expression.Assign(propertySelector.Body, valueParam),
                                propertySelector.Parameters[0], valueParam).Compile();

            var value = getValue(obj);
            // if (value != null && IsJunk(value))
            //     setValue(obj, null);
            return value;
        }
        */




        public static void oldGet(System.Collections.Generic.List<string> ls)
        {
            System.Type targetType = ls.GetType();

            //System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(typeof(string));
            System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(targetType);
            //var prop = System.Linq.Expressions.Expression.Property(p, "Length");

            //System.Linq.Expressions.Expression.Field(p, "fieldName");
            var prop = System.Linq.Expressions.Expression.PropertyOrField(p, "Count");

            // var prop = System.Linq.Expressions.Expression.Property(p, "Count");
            var con = System.Linq.Expressions.Expression.Convert(prop, typeof(object));
            var exp = System.Linq.Expressions.Expression.Lambda(con, p);
            //var func = (System.Func<string, object>)exp.Compile();

            //var func = (System.Func<System.Collections.Generic.List<string>, object>)exp.Compile();

            var func = (System.Func<System.Collections.Generic.List<string>, object>)exp.Compile();



            var obj = ls;
            int len = (int)func(obj);

        }


        public static string PropertyName<TProperty>(System.Linq.Expressions.Expression<System.Func<TProperty>> property)
        {
            System.Linq.Expressions.LambdaExpression lambda = (System.Linq.Expressions.LambdaExpression)property;

            System.Linq.Expressions.MemberExpression memberExpression;
            if (lambda.Body is System.Linq.Expressions.UnaryExpression)
            {
                var unaryExpression = (System.Linq.Expressions.UnaryExpression)lambda.Body;
                memberExpression = (System.Linq.Expressions.MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (System.Linq.Expressions.MemberExpression)lambda.Body;
            }



            return memberExpression.Member.Name;
        }


        public static object GetProperty<T>(T obj, string fieldName)
        {
            System.Func<T, object> getter = GetGetter<T>(fieldName);
            return getter(obj);
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Func<T, object> GetGetter<T>(string fieldName)
        {
            System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(typeof(T));
            System.Linq.Expressions.MemberExpression prop = System.Linq.Expressions.Expression.PropertyOrField(p, fieldName);
            System.Linq.Expressions.UnaryExpression con = System.Linq.Expressions.Expression.Convert(prop, typeof(object));
            System.Linq.Expressions.LambdaExpression exp = System.Linq.Expressions.Expression.Lambda(con, p);

            System.Func<T, object> getter = (System.Func<T, object>)exp.Compile();
            return getter;
        }


        public static TValue GetProperty<T, TValue>(T obj, string fieldName)
        {
            System.Func<T, TValue> getter = GetGetter<T, TValue>(fieldName);
            return getter(obj);
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Func<T, TValue> GetGetter<T, TValue>(string fieldName)
        {
            System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(typeof(T));
            System.Linq.Expressions.MemberExpression prop = System.Linq.Expressions.Expression.PropertyOrField(p, fieldName);
            System.Linq.Expressions.UnaryExpression con = System.Linq.Expressions.Expression.Convert(prop, typeof(TValue));
            System.Linq.Expressions.LambdaExpression exp = System.Linq.Expressions.Expression.Lambda(con, p);

            System.Func<T, TValue> getter = (System.Func<T, TValue>)exp.Compile();
            return getter;
        }


        // https://stackoverflow.com/questions/321650/how-do-i-set-a-field-value-in-an-c-sharp-expression-tree
        public static void SetProperty111<TTarget, TValue>(TTarget target, string fieldName, TValue newValue)
        {
            // Class in which to set value
            System.Linq.Expressions.ParameterExpression targetExp = System.Linq.Expressions.Expression.Parameter(typeof(TTarget), "target");

            // Object's type:
            System.Linq.Expressions.ParameterExpression valueExp = System.Linq.Expressions.Expression.Parameter(typeof(TValue), "value");


            // Expression.Property can be used here as well
            System.Linq.Expressions.MemberExpression memberExp =
                // System.Linq.Expressions.Expression.Field(targetExp, fieldName);
                // System.Linq.Expressions.Expression.Property(targetExp, fieldName);
                System.Linq.Expressions.Expression.PropertyOrField(targetExp, fieldName);


            System.Linq.Expressions.UnaryExpression conversionExp = System.Linq.Expressions.Expression.Convert(valueExp, memberExp.Type);


            System.Linq.Expressions.BinaryExpression assignExp =
                //System.Linq.Expressions.Expression.Assign(memberExp, valueExp); // Without conversion 
                System.Linq.Expressions.Expression.Assign(memberExp, conversionExp);


            System.Action<TTarget, TValue> setter = System.Linq.Expressions.Expression
                .Lambda<System.Action<TTarget, TValue>>(assignExp, targetExp, valueExp).Compile();

            setter(target, newValue);
        }


        public static object FlexibleChangeType(object objVal, System.Type t)
        {
            bool typeIsNullable = (t.IsGenericType && object.ReferenceEquals(t.GetGenericTypeDefinition(), typeof(System.Nullable<>)));
            bool typeCanBeAssignedNull = !t.IsValueType || typeIsNullable;

            if (objVal == null || object.ReferenceEquals(objVal, System.DBNull.Value))
            {
                if (typeCanBeAssignedNull)
                    return null;
                else
                    throw new System.ArgumentNullException("objVal ([DataSource] => SetProperty => FlexibleChangeType => you're trying to assign NULL to a type that NULL cannot be assigned to...)");
            } // End if (objVal == null || object.ReferenceEquals(objVal, System.DBNull.Value))

            // Get base-type
            System.Type tThisType = objVal.GetType();

            if (typeIsNullable)
            {
                t = System.Nullable.GetUnderlyingType(t);
            }


            if (object.ReferenceEquals(tThisType, t))
                return objVal;

            // Convert Guid => string
            if (object.ReferenceEquals(t, typeof(string)) && object.ReferenceEquals(tThisType, typeof(System.Guid)))
            {
                return objVal.ToString();
            }

            // Convert string => Guid 
            if (object.ReferenceEquals(t, typeof(System.Guid)) && object.ReferenceEquals(tThisType, typeof(string)))
            {
                return new System.Guid(objVal.ToString());
            }

            return System.Convert.ChangeType(objVal, t);
        } // End Function MyChangeType


        private static System.Reflection.MethodInfo m_FlexibleChangeType = typeof(LinqReflection).GetMethod("FlexibleChangeType");


        // https://stackoverflow.com/questions/321650/how-do-i-set-a-field-value-in-an-c-sharp-expression-tree
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Action<T, object> GetSetter<T>(string fieldName)
        {
            // Class in which to set value
            System.Linq.Expressions.ParameterExpression targetExp = System.Linq.Expressions.Expression.Parameter(typeof(T), "target");

            // Object's type:
            System.Linq.Expressions.ParameterExpression valueExp = System.Linq.Expressions.Expression.Parameter(typeof(object), "value");


            // Expression.Property can be used here as well
            System.Linq.Expressions.MemberExpression memberExp =
                // System.Linq.Expressions.Expression.Field(targetExp, fieldName);
                // System.Linq.Expressions.Expression.Property(targetExp, fieldName);
                System.Linq.Expressions.Expression.PropertyOrField(targetExp, fieldName);


            // http://www.dotnet-tricks.com/Tutorial/linq/RJX7120714-Understanding-Expression-and-Expression-Trees.html
            System.Linq.Expressions.ConstantExpression targetType = System.Linq.Expressions.Expression.Constant(memberExp.Type);

            // http://stackoverflow.com/questions/913325/how-do-i-make-a-linq-expression-to-call-a-method
            System.Linq.Expressions.MethodCallExpression mce = System.Linq.Expressions.Expression.Call(m_FlexibleChangeType, valueExp, targetType);


            //System.Linq.Expressions.UnaryExpression conversionExp = System.Linq.Expressions.Expression.Convert(valueExp, memberExp.Type);
            System.Linq.Expressions.UnaryExpression conversionExp = System.Linq.Expressions.Expression.Convert(mce, memberExp.Type);


            System.Linq.Expressions.BinaryExpression assignExp =
                //System.Linq.Expressions.Expression.Assign(memberExp, valueExp); // Without conversion 
                System.Linq.Expressions.Expression.Assign(memberExp, conversionExp);

            //System.Action<TTarget, TValue> setter = System.Linq.Expressions.Expression
            System.Action<T, object> setter = System.Linq.Expressions.Expression
                .Lambda<System.Action<T, object>>(assignExp, targetExp, valueExp).Compile();

            return setter;
        }


        public static void SetProperty<T>(T target, string fieldName, object newValue)
        {
            System.Action<T, object> setter = GetSetter<T>(fieldName);
            setter(target, newValue);
        }

    }
}
