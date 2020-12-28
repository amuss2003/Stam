def RelevantChangesFound() {
  def changeLogSets = currentBuild.changeSets
  for (int i = 0; i < changeLogSets.size(); i++) {
      def entries = changeLogSets[i].items
      for (int j = 0; j < entries.length; j++) {
          def entry = entries[j]
          if ("${entry.author}" != "noreply")
            return true;
          //echo "${entry.commitId} by ${entry.author} on ${new Date(entry.timestamp)}: ${entry.msg}"
          /*def files = new ArrayList(entry.affectedFiles)
          for (int k = 0; k < files.size(); k++) {
              def file = files[k]
              echo "  ${file.editType.name} ${file.path}"
          }*/
      }
  }
  return false;
}

pipeline {
  agent any
  triggers {
    pollSCM('H/2 * * * *')
  }
  environment {
    ACR_URL = 'amircontainerregistry.azurecr.io'
    CONTAINER_IMAGE_NAME = 'NoImage'
  }
  parameters {
    choice(name: 'BuildOptions', choices: ['Only if git changes occured', 'Force', 'Skip'], description: 'Build options')
    string(name: 'ManualDeployImage', defaultValue: '', description: 'Manually deploy image name (leave blank to skip deploy)')
  }
  stages {
    stage("build") {
      when {
        expression {
          params.BuildOptions == 'Force' || 
            (params.BuildOptions == 'Only if git changes occured' && RelevantChangesFound())
        }
      }
      steps {      
        script {
          CONTAINER_IMAGE_NAME = "${ACR_URL}/stable/restapi:${BUILD_TIMESTAMP}"
          echo "building image ${CONTAINER_IMAGE_NAME}"
          /*withCredentials([usernamePassword(credentialsId: 'ACR', usernameVariable: 'ACR_USER', passwordVariable: 'ACR_PASSWORD')]) {
            sh "docker login -u $ACR_USER -p $ACR_PASSWORD https://${ACR_URL}"
            def image = docker.build "${CONTAINER_IMAGE_NAME}"
            image.push()
          }*/
        }
      }
    }
    stage("deploy") {
      when {
        expression {
          CONTAINER_IMAGE_NAME != 'NoImage' || params.ManualDeployImage != ''
        }
      }
      steps {
        script {
          //if (params.ManualDeployImage != '')
            env.CONTAINER_IMAGE_NAME = params.ManualDeployImage
          
          echo "deploying image ${CONTAINER_IMAGE_NAME}"
          kubernetesDeploy(
              configs: 'azure-testapp.yaml',
              kubeconfigId: 'KubeConfig',
              enableConfigSubstitution: true
              )   
        }
      }
    }
  }
}
