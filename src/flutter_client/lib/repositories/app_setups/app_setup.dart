import 'package:meta/meta.dart';

@immutable
class AppSetup {
  const AppSetup({required this.id, required this.name, required this.path, this.arguments});

  final String id;
  final String name;
  final String path;
  final String? arguments;
}
