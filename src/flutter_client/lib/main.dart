import 'package:flutter/material.dart';

import 'app_provider.dart';
import 'settings/settings_controller.dart';
import 'settings/settings_service.dart';

void main() async {
  final settingsController = Settings(SettingsService());
  await settingsController.loadSettings();

  runApp(
    AppProvider(
      services: [
        settingsController,
      ],
      child: const MyApp(),
    ),
  );
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    final settings = AppProvider.of<Settings>(context);
    return ListenableBuilder(
      listenable: settings,
      builder: (context, child) {
        return MaterialApp(
          title: 'DevBook',
          theme: ThemeData.light(useMaterial3: true),
          darkTheme: ThemeData.dark(useMaterial3: true),
          themeMode: settings.themeMode,
          home: Dashboard(
            title: 'Flutter Demo Home Page',
            settings: settings,
          ),
        );
      },
    );
  }
}

class Dashboard extends StatelessWidget {
  const Dashboard({
    super.key,
    required this.title,
    required this.settings,
  });

  final String title;
  final Settings settings;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            const Text(
              'Hello',
            ),
            Padding(
              padding: const EdgeInsets.all(16),
              child: DropdownButton<ThemeMode>(
                value: settings.themeMode,
                onChanged: settings.updateThemeMode,
                items: const [
                  DropdownMenuItem(
                    value: ThemeMode.system,
                    child: Text('System Theme'),
                  ),
                  DropdownMenuItem(
                    value: ThemeMode.light,
                    child: Text('Light Theme'),
                  ),
                  DropdownMenuItem(
                    value: ThemeMode.dark,
                    child: Text('Dark Theme'),
                  )
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
