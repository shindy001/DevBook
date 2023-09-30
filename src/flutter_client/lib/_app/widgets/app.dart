import 'package:flutter/material.dart';
import 'package:flutter_client/_app/router.dart';
import 'package:flutter_client/ui/theme/theme.dart';
import 'package:go_router/go_router.dart';

import '../../settings/settings_controller.dart';
import '../app_provider.dart';
import 'title_bar.dart';

class App extends StatelessWidget {
  const App({super.key, this.router});

  final GoRouter? router;

  @override
  Widget build(BuildContext context) {
    final settings = AppProvider.of<Settings>(context);
    return ListenableBuilder(
      listenable: settings,
      builder: (context, child) {
        return MaterialApp.router(
          title: 'DevBook',
          debugShowCheckedModeBanner: false,
          theme: DevBookTheme.themeDataLight,
          darkTheme: DevBookTheme.themeDataDark,
          routerConfig: router ?? createRouter(),
          builder: (context, child) => Scaffold(
            appBar: const TitleBar(),
            body: child,
          ),
        );
      },
    );
  }
}
