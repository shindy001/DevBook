import 'package:flutter/material.dart';

import '../../dashboard/dashboard_page.dart';
import '../../settings/settings_controller.dart';
import '../app_provider.dart';
import 'title_bar.dart';

class App extends StatelessWidget {
  const App({super.key});

  @override
  Widget build(BuildContext context) {
    final settings = AppProvider.of<Settings>(context);
    return ListenableBuilder(
      listenable: settings,
      builder: (context, child) {
        return MaterialApp(
          debugShowCheckedModeBanner: false,
          title: 'DevBook',
          theme: ThemeData.light(useMaterial3: true),
          darkTheme: ThemeData.dark(useMaterial3: true),
          themeMode: settings.themeMode,
          home: Scaffold(
            body: Column(
              children: [
                const TitleBar(),
                Dashboard(
                  title: 'Flutter Demo Home Page',
                  settings: settings,
                ),
              ],
            ),
          ),
        );
      },
    );
  }
}
