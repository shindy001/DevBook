import 'package:flutter/material.dart';
import 'package:flutter_client/repositories/startup_profiles/startup_profiles_repository.dart';
import 'package:flutter_client/ui/widgets/page_with_navigation.dart';

import '../_app/app_provider.dart';
import '../ui/theme/theme.dart';

class StartupProfilesPage extends StatelessWidget {
  const StartupProfilesPage({super.key});

  factory StartupProfilesPage.routeBuilder(_, __) {
    return const StartupProfilesPage(
      key: Key('startupProfiles'),
    );
  }

  @override
  Widget build(BuildContext context) {
    final startupProfilesRepository = AppProvider.of<StartupProfilesRepository>(context);

    return ListenableBuilder(
      listenable: startupProfilesRepository,
      builder: (context, child) => PageWithNavigation(
        activeSideMenuItemName: 'Startup Profiles',
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Padding(
              padding: const EdgeInsets.all(DevBookSpacing.sm),
              child: Text(
                'Startup Profiles',
                style: Theme.of(context).textTheme.headlineLarge,
              ),
            ),
            Row(
              children: [
                Expanded(
                  child: Card(
                    clipBehavior: Clip.antiAlias,
                    child: SingleChildScrollView(
                      child: DataTable(
                        columnSpacing: 20,
                        columns: const [
                          DataColumn(label: Text('Id')),
                          DataColumn(label: Text('Name')),
                          DataColumn(label: Text('AppSetupIds')),
                          DataColumn(label: Text('Actions')),
                        ],
                        rows: [
                          for (var item in startupProfilesRepository.startupProfiles)
                            DataRow(cells: [
                              DataCell(Text(item.id)),
                              DataCell(Text(item.name)),
                              DataCell(ConstrainedBox(
                                constraints: const BoxConstraints(maxWidth: 200),
                                child: Text(
                                  item.appSetupsIds?.join(', ') ?? '',
                                  overflow: TextOverflow.ellipsis,
                                ),
                              )),
                              const DataCell(Text('')),
                            ])
                        ],
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
