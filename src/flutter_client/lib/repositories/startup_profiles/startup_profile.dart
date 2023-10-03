import 'package:meta/meta.dart';

@immutable
class StartupProfile {
  const StartupProfile({required this.id, required this.name, this.appSetupsIds});

  final String id;
  final String name;
  final List<String>? appSetupsIds;
}
