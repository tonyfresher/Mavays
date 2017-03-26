# SMWHR

### Help: 

[Курс по разработке от Технопарка Mail.Ru](youtube.com/playlist?list=PLrCZzMib1e9qLzDXvYnpnJdUsGr3t7fSu), [Android Guide](developer.android.com), [Xamarin Guide](developer.xamarin.com), [Firebase Егорова](github.com/xoposhiy/firebase-course)

### Начало работы

1. Делаем fork (будем называть его origin)
2. Копируем его себе ( `git clone git@github.com:yourname/SMWHR.git` )
3. Добавляем удаленный репозиторий, upstream ( `git remote add upstream git@github.com:tonyfresher/SMWHR.git` )
4. Делаем ветку ( `git checkout -b branch_name` )
5. Делаем изменения

### Вносим изменения в upstream

1. Добавляем изменения локально ( `git add .` )
2. Делаем коммит ( `git commit -m "Хорошее осмысленное сообщение"` )
3. Фиксируем изменения в origin ( `git push -u origin branch_name` )
4. Делаем пулл реквест через интерфейс github (your fork -> `branches` -> напротив вашей ветки `new pull request`)

### Забираем изменения с upstream

Если upstream изменился, во время того, как мы делаем изменения в origin, чтобы сделать ПР, нужно чтобы наш origin был актуальным. Для этого:

1. Забираем изменения с upstream `git pull --rebase upstream master`
2. Делаем перепуш `git push origin branch_name -f`
