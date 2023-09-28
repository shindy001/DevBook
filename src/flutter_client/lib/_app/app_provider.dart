import 'package:flutter/material.dart';

class AppProvider extends InheritedWidget {
  const AppProvider({super.key, required super.child, this.services});

  final List<Object>? services;

  static T of<T>(BuildContext context) {
    final AppProvider? result = context.dependOnInheritedWidgetOfExactType<AppProvider>();
    assert(result != null, 'No Settings found in context');
    final service = result?.services?.whereType<T>().first;
    assert(service != null, "No Service '$service' found in context");
    return service!;
  }

  @override
  bool updateShouldNotify(AppProvider oldWidget) => services != oldWidget.services;
}
