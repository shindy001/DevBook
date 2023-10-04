import 'package:flutter/material.dart';
import 'package:flutter_client/_app/app_provider.dart';
import 'package:flutter_client/repositories/app_setups/app_setups_repository.dart';
import 'package:flutter_client/ui/theme/dev_book_spacing.dart';
import 'package:flutter_client/ui/widgets/page_with_navigation.dart';

class AppSetupsPage extends StatelessWidget {
  const AppSetupsPage({super.key});

  factory AppSetupsPage.routeBuilder(_, __) {
    return const AppSetupsPage(
      key: Key('appSetups'),
    );
  }

  @override
  Widget build(BuildContext context) {
    final appSetupsRepository = AppProvider.of<AppSetupsRepository>(context);

    return ListenableBuilder(
      listenable: appSetupsRepository,
      builder: (context, child) => PageWithNavigation(
        activeSideMenuItemName: 'App Setups',
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Padding(
              padding: const EdgeInsets.all(DevBookSpacing.sm),
              child: Text(
                'App Setups',
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
                          DataColumn(label: Text('Path')),
                          DataColumn(label: Text('Arguments')),
                          DataColumn(label: Text('Actions')),
                        ],
                        rows: [
                          for (var item in appSetupsRepository.appSetups)
                            DataRow(cells: [
                              DataCell(Text(item.id)),
                              DataCell(Text(item.name)),
                              DataCell(Text(item.path)),
                              DataCell(Text(item.arguments ?? '')),
                              const DataCell(Text('')),
                            ])
                        ],
                      ),
                    ),
                  ),
                ),
              ],
            )
          ],
        ),
      ),
    );
  }
}
